using Grpc.Net.Client;
using TechTrack.UserService.Protos;

namespace TechTrack.UserService
{
    public class AuthClient
    {
        private readonly AuthService.AuthServiceClient _client;

        public AuthClient()
        {
            _client = new AuthService.AuthServiceClient(GrpcChannel.ForAddress("http://auth-service:8080"));
        }

        public async Task<string> SayHelloAsync(string name)
        {
            var response = await _client.SayHelloAsync(new HelloRequest { Name = name });
            return response.Message;
        }
    }
}
