using FrontendShared.ViewModels;
using ReactiveUI.SourceGenerators;

namespace FrontendShared.Services.Grpc;

public interface IGrpcService
{
    Uri Address { get; set; }

    string? JwtToken { get; set; }
}

public partial class GrpcService : ViewModelBase, IGrpcService
{
    public Uri Address { get; set; } = new("http://localhost:8080");

    [Reactive]
    public partial string? JwtToken { get; set; }
}