using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BlazorAuthDemo.Models
{
    public class User
    {
        public string DisplayName { get; set; }
        public string Mail { get; set; }

        [JsonPropertyName("value")]
        public List<string> Groups { get; set; }
    }
}
