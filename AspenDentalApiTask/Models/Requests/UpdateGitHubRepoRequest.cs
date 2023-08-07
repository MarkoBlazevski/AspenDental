using Newtonsoft.Json;

namespace AspenDentalApiTask.Models.Requests
{
    public class UpdateGitHubRepoRequest
    {
        [JsonProperty("private")]
        public bool Private { get; set; }

        [JsonProperty("site_admin")]
        public bool SiteAdmin { get; set; }
    }
}
