using System.Reflection;
using FrontendShared.Services;
using FrontendShared.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FrontendShared.Extensions;

public static class AvaloniaServiceExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddViewModels(Assembly assembly)
        {
            Assembly[] assemblies = [typeof(MainWindowViewModel).Assembly, assembly];

            IEnumerable<Type> viewModels = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(ViewModelBase)) && t is { IsClass: true, IsAbstract: false });

            foreach (Type viewModel in viewModels) services.AddSingleton(viewModel);

            return services;
        }

        public IServiceCollection AddApplicationServices()
        {
            IEnumerable<Type> types = typeof(INavigationService).Assembly.GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false });

            foreach (Type implementation in types)
            {
                Type? interfaceType = implementation.GetInterfaces()
                    .FirstOrDefault(i =>
                        i.Name == $"I{implementation.Name}" &&
                        (i.Name.EndsWith("Service") || i.Name.EndsWith("Factory"))
                    );

                if (interfaceType is null)
                    continue;

                services.AddSingleton(interfaceType, implementation);
            }

            return services;
        }
    }
}