using System.Collections.Generic;

namespace BlazorAuthDemo.Models.Requests
{
    public class BatchRequest
    {
        public List<Request> Requests { get; set; }
    }
}
