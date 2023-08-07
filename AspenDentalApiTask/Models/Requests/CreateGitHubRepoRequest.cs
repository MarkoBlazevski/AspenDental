using Newtonsoft.Json;

namespace AspenDentalApiTask.Models.Requests
{
    public class CreateGitHubRepoRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
