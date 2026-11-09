using FrontendShared.Models.Sidebar;
using ReactiveUI.SourceGenerators;

namespace FrontendShared.ViewModels.Sidebar;

public partial class SidebarEntryViewModel : ViewModelBase
{
    [Reactive]
    public partial SidebarEntryModel Data { get; private set; }

    public SidebarEntryViewModel(SidebarEntryModel data)
    {
        Data = data;
    }
}