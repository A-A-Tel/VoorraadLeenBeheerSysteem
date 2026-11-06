using FrontendShared.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FrontendShared.Services;

public interface IViewModelFactory
{
    TViewModel Get<TViewModel>() where TViewModel : ViewModelBase;
}

public class ViewModelFactory(IServiceProvider services) : IViewModelFactory
{
    public TViewModel Get<TViewModel>() where TViewModel : ViewModelBase
    {
        return services.GetRequiredService<TViewModel>();
    }
}