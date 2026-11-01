using System.Threading.Tasks;
using FrontendShared.ViewModels;

namespace FrontendLender.ViewModels.Login;

public class LoginViewModel : PageViewModelBase
{
    public override Task LoadAsync()
    {
        return Task.CompletedTask;
    }
}