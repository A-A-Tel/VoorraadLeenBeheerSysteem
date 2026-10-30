namespace FrontendShared.Services;

public interface IGrpcService
{
    Uri Address { get; set; }

    string? JwtToken { get; set; }
}

public class GrpcService : IGrpcService
{
    public Uri Address { get; set; } = new("http://localhost:8080");

    public string? JwtToken { get; set; }
}