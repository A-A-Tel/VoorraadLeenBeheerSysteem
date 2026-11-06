using ReactiveUI;

namespace FrontendShared.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ViewModelBase? CurrentPage
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = null;
}