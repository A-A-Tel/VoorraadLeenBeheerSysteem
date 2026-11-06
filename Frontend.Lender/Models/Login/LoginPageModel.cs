namespace FrontendLender.Models.Login;

public class LoginPageModel
{
    public required byte[] CardBytes { get; init; }
    public required string Name { get; init; }
}