using Grpc.Net.Client;

namespace FrontendShared.Services;

public class GrpcService
{
    public Uri Address { get; set; } = new("http://localhost:8080");

    public string? JwtToken { get; set; }
}