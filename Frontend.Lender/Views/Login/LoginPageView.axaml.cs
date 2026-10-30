using Avalonia.Controls;
using Avalonia.Interactivity;
using FrontendLender.ViewModels;

namespace FrontendLender.Views.Login;

public partial class LoginPageView : UserControl
{
    public LoginPageView()
    {
        InitializeComponent();
    }

    private void PasswordVisibility(object sender, RoutedEventArgs e)
    {
        if (PasswordField.PasswordChar == '\0')
        {
            PasswordField.PasswordChar = '•';
            Icon.Path = "../../Assets/Icons/Eye.svg";
        }
        else
        {
            PasswordField.PasswordChar = '\0';
            Icon.Path = "../../Assets/Icons/EyeClosed.svg";
        }
    }
}