using Backend.Database.Entities;
using Protos.Auth;
using Protos.Role;

namespace Backend.Grpc.AuthLogic;

public class AuthService(AuthRepository repository)
{
    public async Task<LenderLookupResponse> GetLenderName(LenderLookupRequest request)
    {
        User? user = await repository.GetUser(request.CardBytes.ToByteArray());

        if (user is null or { Role.Id: RoleType.Borrower })
        {
            throw new KeyNotFoundException("Lender does not exist");
        }

        return new LenderLookupResponse
        {
            Name = user.Name
        };
    }

    public async Task<LenderLoginRequest> Login()
    {
        return null!;
    }
}