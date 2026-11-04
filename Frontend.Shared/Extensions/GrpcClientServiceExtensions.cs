using FrontendShared.Interceptors;
using FrontendShared.Services;
using FrontendShared.Services.Grpc;
using Microsoft.Extensions.DependencyInjection;
using Protos.Auth;

namespace FrontendShared.Extensions;

public static class GrpcClientServiceExtensions
{
    public static IServiceCollection AddGrpcClients(this IServiceCollection services)
    {
        services
            .AddGrpcClient<AuthController.AuthControllerClient>((sp, o) =>
            {
                IGrpcService grpcService = sp.GetRequiredService<IGrpcService>();
                o.Address = grpcService.Address;
            })
            .AddInterceptor<JwtInterceptor>();

        services
            .AddGrpcClient<AuthController.AuthControllerClient>((sp, o) =>
            {
                IGrpcService grpcService = sp.GetRequiredService<IGrpcService>();
                o.Address = grpcService.Address;
            })
            .AddInterceptor<JwtInterceptor>();

        services
            .AddGrpcClient<AuthController.AuthControllerClient>((sp, o) =>
            {
                IGrpcService grpcService = sp.GetRequiredService<IGrpcService>();
                o.Address = grpcService.Address;
            })
            .AddInterceptor<JwtInterceptor>();

        return services
            .AddSingleton<JwtInterceptor>();
    }
}