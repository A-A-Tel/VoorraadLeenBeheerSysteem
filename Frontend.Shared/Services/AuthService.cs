namespace FrontendShared.Services;

public interface IAuthService
{
    bool ManagerLogin(string email, string password);

    bool LenderLogin(byte[] cardBytes, string password);

    string LenderLookup(byte[] cardBytes);
}

public class AuthService() : IAuthService
{
    public bool ManagerLogin(string email, string password)
    {
        return false;
    }

    public bool LenderLogin(byte[] cardBytes, string password)
    {
        return false;
    }

    public string LenderLookup(byte[] cardBytes)
    {
        return null!;
    }
}