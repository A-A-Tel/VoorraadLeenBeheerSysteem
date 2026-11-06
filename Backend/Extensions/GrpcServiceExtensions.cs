using System.Reflection;
using Backend.Grpc.Logic.AuthLogic;
using Backend.Grpc.Logic.UserLogic;
using Grpc.Core;

namespace Backend.Extensions;

public static class GrpcServiceExtensions
{
    public static IEndpointRouteBuilder MapGrpcControllers(this IEndpointRouteBuilder builder, Assembly assembly)
    {
        builder.MapGrpcService<AuthController>();
        builder.MapGrpcService<UserController>();

        return builder;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, Assembly assembly)
    {
        IEnumerable<Type> types = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false });

        foreach (Type implementation in types)
        {
            Type? interfaceType = implementation.GetInterfaces()
                .FirstOrDefault(i =>
                    i.Name == $"I{implementation.Name}" &&
                    (i.Name.EndsWith("Service") ||
                     i.Name.EndsWith("Repository"))
                    );

            if (interfaceType is null)
                continue;

            services.AddScoped(interfaceType, implementation);
        }

        return services;
    }
}