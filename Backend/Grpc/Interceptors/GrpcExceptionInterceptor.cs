using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Backend.Grpc.Interceptors;

public class GrpcExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (KeyNotFoundException ex)
        {
            // HTTP 404
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
        catch (ArgumentException ex)
        {
            // HTTP 400
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
        }
        catch (UnauthorizedAccessException ex)
        {
            // HTTP 401
            throw new RpcException(new Status(StatusCode.Unauthenticated, ex.Message));
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception ex)
        {
            // HTTP 500
            throw new RpcException(new Status(StatusCode.Internal,
                "An internal server error occurred. Please try again later."));
        }
    }
}