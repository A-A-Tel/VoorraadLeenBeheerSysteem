using System.Threading.Tasks;
using System.Windows.Input;
using FrontendLender.Models.Login;
using FrontendShared.ViewModels;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace FrontendLender.ViewModels.Login;

public partial class LoginPageViewModel : PageViewModelBase<LoginPageModel>
{
    public LoginPageModel Data { get; private set; } = null!;

    [Reactive]
    public partial string Password { get; set; } = "";

    public ICommand LoginCommand { get; private set; } = null!;

    public override Task LoadAsync(LoginPageModel model)
    {
        LoginCommand = ReactiveCommand.CreateFromTask(async () =>
        {

        });

        Data = model;
        Password = "";
        return Task.CompletedTask;
    }
}