using System.Net;

namespace DotNetCore_New.Model
{
    public class APIResponse
    {
        public bool Status { get; set; }
        public string StatusMessage { get; set; } = string.Empty;
        public HttpStatusCode StatusCode { get; set; }
        public dynamic Data { get; set; }
        public List<string> Errors { get; set; }
    }
}
