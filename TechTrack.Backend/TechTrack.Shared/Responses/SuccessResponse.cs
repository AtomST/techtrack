using System.Net;

namespace TechTrack.Shared.Responses
{
    public class SuccessResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public object? Data { get; set; }
    }

}
