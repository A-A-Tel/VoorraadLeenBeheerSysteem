using System.Threading.Tasks;
using FrontendShared.Services.Grpc;
using FrontendShared.ViewModels;
using Microsoft.AspNetCore.SignalR;
using ReactiveUI.SourceGenerators;

namespace FrontendLender.ViewModels.Dashboard;

public partial class DashboardPageViewModel(
    IGrpcService grpcService
    ) : PageViewModelBase
{
    [Reactive]
    public partial string JwtToken { get; private set; } = null!;

    public override Task LoadAsync()
    {
        JwtToken = grpcService.JwtToken?? "No token";

        return Task.CompletedTask;
    }
}