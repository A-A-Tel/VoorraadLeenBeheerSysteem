using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Protos.Auth;

namespace Backend.Grpc.AuthLogic;

public class AuthController(IAuthService service) : Protos.Auth.AuthController.AuthControllerBase
{
    [AllowAnonymous]
    public override Task<LoginResponse> ManagerLogin(ManagerLoginRequest request, ServerCallContext context)
    {
        return service.LoginAsync(request);
    }

    [AllowAnonymous]
    public override Task<LoginResponse> LenderLogin(LenderLoginRequest request, ServerCallContext context)
    {
        return service.LoginAsync(request);
    }

    [AllowAnonymous]
    public override Task<LenderLookupResponse> LenderLookup(LenderLookupRequest request, ServerCallContext context)
    {
        return service.GetLenderName(request);
    }
}