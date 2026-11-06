using System.Threading.Tasks;
using System.Windows.Input;
using FrontendLender.Models.Login;
using FrontendLender.ViewModels.Dashboard;
using FrontendLender.ViewModels.LoginScan;
using FrontendShared.Services;
using FrontendShared.Services.Grpc;
using FrontendShared.ViewModels;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace FrontendLender.ViewModels.Login;

public partial class LoginPageViewModel(
    IAuthService authService,
    INavigationService navigationService
    ) : PageViewModelBase<LoginPageModel>
{
    public LoginPageModel Data { get; private set; } = null!;

    [Reactive]
    public partial string Password { get; set; } = "";

    private async Task Login()
    {
        bool success = await authService.LenderLogin(Data.CardBytes, Password);

        if (success)
        {
            await navigationService.NavigateToAsync<DashboardPageViewModel>();
        }
        else
        {
            await navigationService.NavigateToAsync<LoginScanPageViewModel>();
        }
    }
    public ICommand LoginCommand { get; private set; } = null!;

    private async Task Cancel()
    {
        await navigationService.NavigateToAsync<LoginScanPageViewModel>();
    }
    public ICommand CancelCommand { get; private set; } = null!;

    public override Task LoadAsync(LoginPageModel model)
    {
        LoginCommand = ReactiveCommand.CreateFromTask(Login);
        CancelCommand = ReactiveCommand.CreateFromTask(Cancel);

        Data = model;
        Password = "";
        return Task.CompletedTask;
    }
}