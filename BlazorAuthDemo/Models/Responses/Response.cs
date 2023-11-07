namespace BlazorAuthDemo.Models.Responses
{
    public class Response
    {
        public string Id { get; set; }
        public int Status { get; set; }
        public object Headers { get; set; }
        public object Body { get; set; }
    }
}
