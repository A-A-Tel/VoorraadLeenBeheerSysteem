using System.Reflection;
using Grpc.Core;

namespace Backend.Extensions;

public static class GrpcServiceExtensions
{
    public static IEndpointRouteBuilder MapGrpcControllers(this IEndpointRouteBuilder builder, Assembly assembly)
    {
        MethodInfo? mapGrpcMethod = typeof(GrpcEndpointRouteBuilderExtensions)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(m =>
                m is { Name: nameof(GrpcEndpointRouteBuilderExtensions.MapGrpcService), IsGenericMethod: true }
                && m.GetParameters().Length == 1
                && m.GetParameters()[0].ParameterType == typeof(IEndpointRouteBuilder));

        if (mapGrpcMethod == null) throw new InvalidOperationException("GRPC map method not found");

        IEnumerable<Type> controllers = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false })
            .Where(t =>
            {
                Type? baseType = t.BaseType;
                while (baseType != null && baseType != typeof(object))
                {
                    if (baseType.GetCustomAttribute<BindServiceMethodAttribute>() != null ||
                        baseType.DeclaringType?.GetCustomAttribute<BindServiceMethodAttribute>() != null)
                        return true;

                    baseType = baseType.BaseType;
                }

                return false;
            });

        foreach (Type controller in controllers)
        {
            MethodInfo genericMethod = mapGrpcMethod.MakeGenericMethod(controller);
            genericMethod.Invoke(null, [builder]);
        }

        return builder;
    }

    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        Assembly assembly)
    {
        Type[] types = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false })
            .ToArray();

        foreach (Type implementation in types)
        {
            Type? interfaceType = implementation.GetInterfaces()
                .FirstOrDefault(i =>
                    i.Name == $"I{implementation.Name}" &&
                    (i.Name.EndsWith("Service") ||
                     i.Name.EndsWith("Repository")));

            if (interfaceType is null)
                continue;

            services.AddScoped(interfaceType, implementation);
        }

        return services;
    }
}