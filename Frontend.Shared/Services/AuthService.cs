using Google.Protobuf;
using Grpc.Core;
using Protos.Auth;

namespace FrontendShared.Services;

public interface IAuthService
{
    bool ManagerLogin(string email, string password);

    bool LenderLogin(byte[] cardBytes, string password);

    Task<string?> LenderLookup(byte[] cardBytes);
}

public class AuthService(AuthController.AuthControllerClient client, INavigationService navigationService) : IAuthService
{
    public bool ManagerLogin(string email, string password)
    {
        return false;
    }

    public bool LenderLogin(byte[] cardBytes, string password)
    {
        return false;
    }

    public async Task<string?> LenderLookup(byte[] cardBytes)
    {
        LenderLookupRequest request = new()
        {
            CardBytes = ByteString.CopyFrom(cardBytes)
        };

        LenderLookupResponse response;
        try
        {
            response = await client.LenderLookupAsync(request);
        }
        catch (RpcException ex)
        {
            Console.WriteLine($"[AuthService] An error occured in LenderLookup: {ex.Message}");
            return null;
        }

        return response.Name;
    }
}