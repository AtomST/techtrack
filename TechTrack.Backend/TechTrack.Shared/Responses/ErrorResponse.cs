using System.Net;

namespace TechTrack.Shared.Responses
{
    public class ErrorResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public object? Errors { get; set; } = null;
    }
}
