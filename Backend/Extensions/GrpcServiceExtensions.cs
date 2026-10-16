using System.Reflection;
using Grpc.Core;

namespace Backend.Extensions;

public static class GrpcServiceExtensions
{
    extension(IEndpointRouteBuilder builder)
    {
        public IEndpointRouteBuilder MapAllGrpcServices(Assembly assembly)
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
                Console.WriteLine(controller.Name);
                MethodInfo genericMethod = mapGrpcMethod.MakeGenericMethod(controller);
                genericMethod.Invoke(null, [builder]);
            }

            return builder;
        }
    }
}