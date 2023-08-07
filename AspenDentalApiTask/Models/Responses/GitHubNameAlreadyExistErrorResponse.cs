using Newtonsoft.Json;

namespace AspenDentalApiTask.Models.Responses
{
    public class GitHubNameAlreadyExistErrorResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("errors")]
        public List<Error> Errors { get; set; }

        [JsonProperty("documentation_url")]
        public string DocumentationUrl { get; set; }

        public class Error
        {
            [JsonProperty("resource")]
            public string Resource { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("field")]
            public string Field { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }
        }
    }
}
