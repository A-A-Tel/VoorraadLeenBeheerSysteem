using Google.Protobuf;
using Grpc.Core;
using Protos.Auth;

namespace FrontendShared.Services.Grpc;

public interface IAuthService
{
    bool ManagerLogin(string email, string password);

    Task<bool> LenderLogin(byte[] cardBytes, string password);

    Task<string?> LenderLookup(byte[] cardBytes);
}

public class AuthService(
    AuthController.AuthControllerClient client,
    IGrpcService grpcService
    ) : IAuthService
{
    public bool ManagerLogin(string email, string password)
    {
        return false;
    }

    public async Task<bool> LenderLogin(byte[] cardBytes, string password)
    {
        LenderLoginRequest request = new()
        {
            CardBytes = ByteString.CopyFrom(cardBytes),
            Password = password
        };

        try
        {
            LoginResponse response = await client.LenderLoginAsync(request);
            grpcService.JwtToken = response.Jwt;
            return true;
        }
        catch (RpcException ex)
        {
            Console.WriteLine(ex.Message);
        }

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