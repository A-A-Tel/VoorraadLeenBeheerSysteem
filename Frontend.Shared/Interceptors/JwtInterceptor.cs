using Grpc.Core;
using Grpc.Core.Interceptors;

namespace FrontendShared.Interceptors;

public class JwtInterceptor : Interceptor
{
    private readonly Func<string?> _getToken;

    public JwtInterceptor(Func<string?> getToken)
    {
        _getToken = getToken;
    }

    private ClientInterceptorContext<TRequest, TResponse> AddToken<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context)
        where TRequest : class
        where TResponse : class
    {
        Metadata headers = context.Options.Headers ?? [];

        string? token = _getToken();

        if (!string.IsNullOrWhiteSpace(token)) headers.Add("Authorization", $"Bearer {token}");

        CallOptions options = context.Options.WithHeaders(headers);

        return new ClientInterceptorContext<TRequest, TResponse>(
            context.Method,
            context.Host,
            options
        );
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        return continuation(request, AddToken(context));
    }

    public override TResponse BlockingUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        return continuation(request, AddToken(context));
    }

    public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        return continuation(request, AddToken(context));
    }

    public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        return continuation(AddToken(context));
    }

    public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        return continuation(AddToken(context));
    }
}