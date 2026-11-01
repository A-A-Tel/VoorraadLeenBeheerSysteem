using System.Threading.Tasks;
using FrontendLender.Models.Login;
using FrontendLender.ViewModels.Login;
using FrontendShared.Services;
using FrontendShared.ViewModels;

namespace FrontendLender.ViewModels.LoginScan;

public class LoginScanPageViewModel(INavigationService navigationService, IAuthService authService, ISmartCardService smartCardService) : PageViewModelBase
{
    private async Task<bool> LookupCallback(byte[] uid)
    {
        string? name = await authService.LenderLookup(uid);

        if (name is null) return false;

        LoginPageModel model = new()
        {
            CardBytes = uid,
            Name = name
        };

        await navigationService.NavigateToAsync<LoginPageViewModel, LoginPageModel>(model);

        return true;
    }

    public override Task LoadAsync()
    {
        smartCardService.SetCardDetectedCallback(LookupCallback);

        return Task.CompletedTask;
    }
}