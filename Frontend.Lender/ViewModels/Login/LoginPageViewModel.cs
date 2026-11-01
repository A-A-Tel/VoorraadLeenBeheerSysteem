using System.Threading.Tasks;
using FrontendLender.Models.Login;
using FrontendShared.ViewModels;

namespace FrontendLender.ViewModels.Login;

public class LoginPageViewModel : PageViewModelBase<LoginPageModel>
{
    public override Task LoadAsync(LoginPageModel? model)
    {
        return Task.CompletedTask;
    }
}