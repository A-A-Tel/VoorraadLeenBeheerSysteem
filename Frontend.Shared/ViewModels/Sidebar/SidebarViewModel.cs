using System.Collections.ObjectModel;
using FrontendShared.Models.Sidebar;

namespace FrontendShared.ViewModels.Sidebar;

public partial class SidebarViewModel : ViewModelBase
{
    public ObservableCollection<SidebarEntryViewModel> SidebarEntries { get; } = [];

    public void AddEntry(SidebarEntryModel data)
    {
        SidebarEntries.Add(new SidebarEntryViewModel(data));
    }
}