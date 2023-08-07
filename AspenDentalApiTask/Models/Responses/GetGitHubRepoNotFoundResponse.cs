using Newtonsoft.Json;

namespace AspenDentalApiTask.Models.Responses
{
    public class GetGitHubRepoNotFoundResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("documentation_url")]
        public string Documentation_url { get; set; }
    }
}
