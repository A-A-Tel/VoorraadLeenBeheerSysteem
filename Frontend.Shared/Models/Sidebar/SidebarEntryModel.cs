using FrontendShared.Services;

namespace FrontendShared.Models.Sidebar;

public class SidebarEntryModel
{
    public required string IconPath
    {
        get => $"avares://FrontendLender/Assets/{field.Trim('/')}";
        init;
    }
    public required string DisplayName { get; init; }
    public required Func<INavigationService, Task> NavigateToAsync { get; init; }
}