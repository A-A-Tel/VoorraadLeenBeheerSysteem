using FrontendShared.ViewModels;

namespace FrontendShared.Services;

public interface INavigationService
{
    Task NavigateToAsync<TViewModel>() where TViewModel : PageViewModelBase;

    Task NavigateToAsync<TViewModel, TModel>(TModel? model = null)
        where TViewModel : PageViewModelBase<TModel>
        where TModel : class;
}

public class NavigationService(
    MainWindowViewModel mainWindow,
    IViewModelFactory viewModelFactory,
    ISmartCardService smartCardService) : INavigationService
{
    public async Task NavigateToAsync<TViewModel>()
        where TViewModel : PageViewModelBase
    {
        smartCardService.SetCardDetectedCallback(null);

        PageViewModelBase vm = viewModelFactory.Get<TViewModel>();
        await vm.LoadAsync();

        mainWindow.CurrentPage = vm;
    }

    public async Task NavigateToAsync<TViewModel, TModel>(TModel? model = null)
        where TViewModel : PageViewModelBase<TModel>
        where TModel : class
    {
        smartCardService.SetCardDetectedCallback(null);

        PageViewModelBase<TModel> vm = viewModelFactory.Get<TViewModel>();
        await vm.LoadAsync(model);

        mainWindow.CurrentPage = vm;
    }
}