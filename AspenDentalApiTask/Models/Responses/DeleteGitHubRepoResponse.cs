using Newtonsoft.Json;

namespace AspenDentalApiTask.Models.Responses
{
    public class DeleteGitHubRepoResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("documentation_url")]
        public string DocumentationUrl { get; set; }
    }
}
