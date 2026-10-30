using FrontendShared.Interceptors;
using FrontendShared.Services;
using Microsoft.Extensions.DependencyInjection;
using Protos.Auth;
using Protos.Product;
using Protos.User;

namespace FrontendShared.Extensions;

public static class GrpcClientServiceExtensions
{
    public static IServiceCollection AddGrpc(this IServiceCollection services)
    {
        services.AddSingleton<GrpcService>();

        services
            .AddGrpcClient<AuthController.AuthControllerClient>((sp, o) =>
            {
                GrpcService grpcService = sp.GetRequiredService<GrpcService>();
                o.Address = grpcService.Address;
            })
            .AddInterceptor<JwtInterceptor>();

        services
            .AddGrpcClient<AuthController.AuthControllerClient>((sp, o) =>
            {
                GrpcService grpcService = sp.GetRequiredService<GrpcService>();
                o.Address = grpcService.Address;
            })
            .AddInterceptor<JwtInterceptor>();

        services
            .AddGrpcClient<AuthController.AuthControllerClient>((sp, o) =>
            {
                GrpcService grpcService = sp.GetRequiredService<GrpcService>();
                o.Address = grpcService.Address;
            })
            .AddInterceptor<JwtInterceptor>();

        return services;
    }
}