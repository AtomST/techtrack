using Grpc.Core;
using TechTrack.AuthService.Protos;

namespace TechTrack.AuthService.Logic.gRPC
{
    public class AuthGrpcLogic : Protos.AuthService.AuthServiceBase
    {
        public override Task<HelloResponse> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(
                new HelloResponse()
                {
                    Message = $"Hello {request.Name}"
                });
        }
    }
}
