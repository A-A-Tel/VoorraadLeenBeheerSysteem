using Grpc.Core;
using Protos.Auth;

namespace Backend.Grpc.Auth;

public class AuthController : Protos.Auth.AuthController.AuthControllerBase
{
    public override Task<LoginResponse> LenderLogin(LenderLoginRequest request, ServerCallContext context)
    {
        return null!;
    }
}