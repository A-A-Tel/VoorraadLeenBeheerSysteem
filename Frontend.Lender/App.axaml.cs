using System.Reflection;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FrontendLender.ViewModels.LoginScan;
using FrontendShared.Extensions;
using FrontendShared.Services;
using FrontendShared.ViewModels;
using FrontendShared.Views;
using Microsoft.Extensions.DependencyInjection;

namespace FrontendLender;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        ServiceCollection serviceCollection = new();
        Assembly assembly = GetType().Assembly;

        serviceCollection
            .AddViewModels(assembly)
            .AddApplicationServices()
            .AddGrpcClients();

        ServiceProvider services = serviceCollection.BuildServiceProvider();
        MainWindowViewModel main = services.GetRequiredService<MainWindowViewModel>();
        INavigationService navigation = services.GetRequiredService<INavigationService>();
        navigation.NavigateToAsync<LoginScanPageViewModel>().GetAwaiter().GetResult();

        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                desktop.MainWindow = new MainWindowView { DataContext = main };
                break;
            case ISingleViewApplicationLifetime single:
                single.MainView = new MainWindowView { DataContext = main };
                break;
        }

        base.OnFrameworkInitializationCompleted();
    }
}