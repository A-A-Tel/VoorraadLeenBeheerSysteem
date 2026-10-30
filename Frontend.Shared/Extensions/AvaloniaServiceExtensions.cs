using System.Reflection;
using FrontendShared.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FrontendShared.Extensions;

public static class AvaloniaServiceExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddViewModels(Assembly assembly)
        {
            IEnumerable<Type> viewModels = assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ViewModelBase)) && t is { IsClass: true, IsAbstract: false });

            foreach (Type viewModel in viewModels)
            {
                services.AddScoped(viewModel);
            }

            return services;
        }

        public IServiceCollection AddApplicationServices(Assembly assembly)
        {
            IEnumerable<Type> types = assembly.GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false });

            foreach (Type implementation in types)
            {
                Type? interfaceType = implementation.GetInterfaces()
                    .FirstOrDefault(i =>
                        i.Name == $"I{implementation.Name}" &&
                        i.Name.EndsWith("Service"));

                if (interfaceType is null)
                    continue;

                services.AddScoped(interfaceType, implementation);
            }

            return services;
        }
    }
}