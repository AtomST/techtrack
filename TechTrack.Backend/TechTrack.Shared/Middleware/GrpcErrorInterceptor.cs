using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using TechTrack.Shared.Exceptions;

namespace TechTrack.Shared.Middleware
{
    public class GrpcErrorInterceptor : Interceptor
    {
        private readonly ILogger _logger;
        public GrpcErrorInterceptor(ILogger<GrpcErrorInterceptor> logger) 
        { 
            _logger = logger;
        }
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var call = continuation(request, context);

            return new AsyncUnaryCall<TResponse>(
                HandleErrors(call.ResponseAsync),
                call.ResponseHeadersAsync,
                call.GetStatus,
                call.GetTrailers,
                call.Dispose);
           
        }

        private async Task<TResponse> HandleErrors<TResponse>(Task<TResponse> task)
        {
            try
            {
                return await task;
            }
            catch (RpcException ex)
            {
                throw ex.StatusCode switch
                {
                    StatusCode.Unavailable => new ServiceUnavailableException("Сервис недоступен"),
                    StatusCode.NotFound => new NotFoundException(ex.Status.Detail),
                    StatusCode.PermissionDenied => new ForbiddenException(ex.Status.Detail),
                    StatusCode.Unauthenticated => new UnauthorizedException(ex.Status.Detail),
                    StatusCode.AlreadyExists => new RecordExistsException(ex.Status.Detail),
                    StatusCode.InvalidArgument => new InvalidInputException(ex.Status.Detail),
                    _ => new Exception(ex.Status.Detail)
                };
            }
        }
    }
}
