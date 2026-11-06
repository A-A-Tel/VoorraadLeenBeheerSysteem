using Backend.Database.Entities;
using Microsoft.AspNetCore.Identity;

namespace Backend.Grpc.Universal;

public interface IPasswordService
{
    string HashPassword(string password, User? user = null);
    PasswordVerificationResult VerifyPassword(User user, string hashedPassword, string providedPassword);
}

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public string HashPassword(string password, User? user = null)
    {
        return _passwordHasher.HashPassword(user!, password);
    }

    public PasswordVerificationResult VerifyPassword(User user, string hashedPassword, string providedPassword)
    {
        return _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
    }
}