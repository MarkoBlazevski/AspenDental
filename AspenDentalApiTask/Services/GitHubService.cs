using AspenDentalApiTask.Clients;
using AspenDentalApiTask.Models.Responses;
using AspenDentalApiTask.Setup;
using BoDi;
using Newtonsoft.Json;
using System.Net;

namespace AspenDentalApiTask.Services
{
    public class GitHubService
    {
        private readonly GitHubApiClient _gitHubApiClient;
        private readonly IObjectContainer _objectContainer;
        public Variables _variables;

        public GitHubService(GitHubApiClient gitHubApiClient, IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
            _gitHubApiClient = gitHubApiClient;
            _variables = _objectContainer.Resolve<Variables>();
        }

        public async Task<(HttpStatusCode ResponseHttp, CreateGitHubRepoResponse ResponseBody)> CreateRepoAsync(object body)
        {
            var response = await _gitHubApiClient.PostRequestAsync(_variables.CreateRepoEndpoint, body);

            // Log the status code and response content
            Console.WriteLine($"Response Status Code: {response.StatusCode}");
            Console.WriteLine($"Response Content: {response.Content}");

            // Throw an exception if it's not a success status code
            if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new HttpRequestException(response.Content, null, response.StatusCode);
            }

            var responseBody = JsonConvert.DeserializeObject<CreateGitHubRepoResponse>(response.Content);

            return (response.StatusCode, responseBody);
        }

        public async Task<(HttpStatusCode ResponseHttp, GetGitHubRepoResponse ResponseBody)> GetRepoAsync(string owner, string repo)
        {
            string endpoint = _variables.GetRepoEndpoint + $"{owner}/{repo}";
            var response = await _gitHubApiClient.GetRequestAsync(endpoint);


            // Log the status code and response content
            Console.WriteLine($"Response Status Code: {response.StatusCode}");
            Console.WriteLine($"Response Content: {response.Content}");

            var responseBody = JsonConvert.DeserializeObject<GetGitHubRepoResponse>(response.Content);

            return (response.StatusCode, responseBody);
        }

        public async Task<(HttpStatusCode ResponseHttp, UpdateGitHubRepoResponse ResponseBody)> UpdateRepoAsync(string owner, string repo, object body)
        {
            var response = await _gitHubApiClient.PatchRequestAsync($"{owner}/{repo}", body);

            // Log the status code and response content
            Console.WriteLine($"Response Status Code: {response.StatusCode}");
            Console.WriteLine($"Response Content: {response.Content}");

            var responseBody = JsonConvert.DeserializeObject<UpdateGitHubRepoResponse>(response.Content);

            return (response.StatusCode, responseBody);
        }

        public async Task <(HttpStatusCode ResponseHttp, DeleteGitHubRepoResponse ResponseBody)> DeleteRepoAsync(string owner, string repo)
        {
            var response = await _gitHubApiClient.DeleteRequestAsync($"{owner}/{repo}");

            // Log the status code and response content
            Console.WriteLine($"Response Status Code: {response.StatusCode}");
            Console.WriteLine($"Response Content: {response.Content}");

            var responseBody = JsonConvert.DeserializeObject<DeleteGitHubRepoResponse>(response.Content);

            return (response.StatusCode, responseBody);
        }
    }
}
