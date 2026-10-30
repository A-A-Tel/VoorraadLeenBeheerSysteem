using Grpc.Core;
using Protos.Auth;

namespace Backend.Grpc.AuthLogic;

public class AuthController(AuthService service) : Protos.Auth.AuthController.AuthControllerBase
{
    public override Task<LoginResponse> ManagerLogin(ManagerLoginRequest request, ServerCallContext context)
    {
        return service.LoginAsync(request);
    }

    public override Task<LoginResponse> LenderLogin(LenderLoginRequest request, ServerCallContext context)
    {
        return service.LoginAsync(request);
    }

    public override Task<LenderLookupResponse> LenderLookup(LenderLookupRequest request, ServerCallContext context)
    {
        return service.GetLenderName(request);
    }
}