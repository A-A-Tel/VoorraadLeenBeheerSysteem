using System.Threading.Tasks;
using FrontendShared.ViewModels;

namespace FrontendLender.ViewModels.Login;

public class LoginPageViewModel : PageViewModelBase
{
    public override Task LoadAsync()
    {
        return Task.CompletedTask;
    }
}