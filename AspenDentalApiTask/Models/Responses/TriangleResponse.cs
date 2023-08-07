using Newtonsoft.Json;
using System.Net;

namespace AspenDentalApiTask.Models.Responses
{
    public class TriangleResponse
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("errors")]
        public Errors Error { get; set; }
        public class Errors
        {
            [JsonProperty("A")]
            public List<string> A { get; set; }
        }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("traceId")]
        public string TraceId { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
