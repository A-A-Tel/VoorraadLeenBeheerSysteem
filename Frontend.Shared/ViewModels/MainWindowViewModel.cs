using FrontendShared.Services.Grpc;
using FrontendShared.ViewModels.Sidebar;
using ReactiveUI.SourceGenerators;

namespace FrontendShared.ViewModels;

public partial class MainWindowViewModel(
    IGrpcService grpcService
    ) : ViewModelBase
{
    [Reactive]
    public partial ViewModelBase? CurrentPage { get; set; }

    public SidebarViewModel Sidebar { get; } = new();

    public bool IsLoggedIn => grpcService.JwtToken != null;
}