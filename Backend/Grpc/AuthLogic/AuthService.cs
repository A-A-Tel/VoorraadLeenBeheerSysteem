using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Database.Entities;
using Backend.Grpc.Universal;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Protos.Auth;
using Protos.Role;

namespace Backend.Grpc.AuthLogic;

public class AuthService(IAuthRepository repository, IConfiguration configuration, IPasswordService passwordService)
{
    public async Task<LenderLookupResponse> GetLenderName(LenderLookupRequest request)
    {
        User? user = await repository.GetUserAsync(request.CardBytes.ToByteArray());

        if (user is null or { Role.Id: RoleType.Borrower }) throw new KeyNotFoundException("Lender does not exist");

        return new LenderLookupResponse
        {
            Name = user.Name
        };
    }

    public async Task<LoginResponse> LoginAsync(LenderLoginRequest request)
    {
        User? user = await repository.GetUserAsync(request.CardBytes.ToByteArray());

        if (user is null or { Role.Id: RoleType.Borrower }) throw new KeyNotFoundException("Lender does not exist");

        if (user.PasswordHash is null) throw new ArgumentException("Lender does not have a password");

        PasswordVerificationResult verificationResult =
            passwordService.VerifyPassword(user, user.PasswordHash, request.Password);
        if (verificationResult == PasswordVerificationResult.Failed)
            throw new ArgumentException("Incorrect credentials");

        if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
            await repository.RehashPasswordAsync(user, request.Password);

        return new LoginResponse
        {
            Jwt = GenerateJwtToken(user.Id.ToString(), user.Name, user.RoleId)
        };
    }

    public async Task<LoginResponse> LoginAsync(ManagerLoginRequest request)
    {
        User? user = await repository.GetUserAsync(request.Email);

        if (user is null or { Role.Id: RoleType.Borrower } or { PasswordHash: null })
            throw new ArgumentException("Incorrect credentials");

        PasswordVerificationResult verificationResult =
            passwordService.VerifyPassword(user, user.PasswordHash, request.Password);
        if (verificationResult == PasswordVerificationResult.Failed)
            throw new ArgumentException("Incorrect credentials");

        if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
            await repository.RehashPasswordAsync(user, request.Password);

        return new LoginResponse
        {
            Jwt = GenerateJwtToken(user.Id.ToString(), user.Name, user.RoleId)
        };
    }

    private string GenerateJwtToken(string userId, string username, RoleType role)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]!);

        List<Claim> claims =
        [
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Name, username),

            new(ClaimTypes.Role, ((int)role).ToString())
        ];

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(12),
            Issuer = configuration["JwtSettings:Issuer"],
            Audience = configuration["JwtSettings:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}