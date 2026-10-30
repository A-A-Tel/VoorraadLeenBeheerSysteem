namespace FrontendShared.ViewModels;

public abstract class PageViewModelBase : ViewModelBase
{
    public abstract Task LoadAsync();
}