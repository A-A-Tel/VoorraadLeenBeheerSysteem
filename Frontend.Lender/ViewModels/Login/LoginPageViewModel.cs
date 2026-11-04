using System.Threading.Tasks;
using FrontendLender.Models.Login;
using FrontendShared.ViewModels;
using ReactiveUI.SourceGenerators;

namespace FrontendLender.ViewModels.Login;

public partial class LoginPageViewModel : PageViewModelBase<LoginPageModel>
{
    public LoginPageModel Data { get; private set; } = null!;

    [Reactive]
    public partial string Password { get; set; } = "";

    public override Task LoadAsync(LoginPageModel model)
    {
        Data = model;
        Password = "";
        return Task.CompletedTask;
    }
}