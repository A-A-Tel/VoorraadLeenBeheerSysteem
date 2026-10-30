namespace FrontendShared.ViewModels;

public abstract class PageViewModelBase<TModel> : ViewModelBase
    where TModel : class
{
    public abstract Task LoadAsync(TModel? existing);
}