using System.Text.Json.Serialization;

namespace BlazorAuthDemo.Models.Requests
{
    public class Request
    {
        public string Id { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
    }

    public struct Method
    {
        public const string GET = "GET";
        public const string POST = "POST";
        public const string PUT = "PUT";
        public const string DELETE = "DELETE";
    }
}
