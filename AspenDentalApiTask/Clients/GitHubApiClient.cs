using AspenDentalApiTask.Setup;
using BoDi;
using RestSharp;

namespace AspenDentalApiTask.Clients
{
    public class GitHubApiClient : ApiClient
    {
        public Variables _variables;
        readonly IObjectContainer _objectContainer;

        public GitHubApiClient(IObjectContainer objectContainer)
            : base(objectContainer.Resolve<Variables>().GitHubUrl)
        {
            _objectContainer = objectContainer;
            _variables = _objectContainer.Resolve<Variables>();
        }

        public async Task<RestResponse> PostRequestAsync(string endpoint, object body)
        {
            var request = new RestRequest(baseUrl + endpoint, Method.Post);

            request.AddHeader("Accept", _variables.AcceptHeader);
            request.AddHeader("Authorization", _variables.BearerToken);
            request.AddJsonBody(body);

            return await Client.ExecuteAsync(request);
        }

        public async Task<RestResponse> GetRequestAsync(string endpoint)
        {
            var request = new RestRequest(baseUrl + endpoint, Method.Get);

            request.AddHeader("Accept", _variables.AcceptHeader);
            request.AddHeader("Authorization", _variables.BearerToken);

            return await Client.ExecuteAsync(request);
        }

        public async Task<RestResponse> PatchRequestAsync(string endpoint, object body)
        {
            var request = new RestRequest(_variables.UpdateRepoEndpoint + endpoint, Method.Patch);

            request.AddHeader("Accept", _variables.AcceptHeader);
            request.AddHeader("Authorization", _variables.BearerToken);
            request.AddJsonBody(body);

            return await Client.ExecuteAsync(request);
        }

        public async Task<RestResponse> DeleteRequestAsync(string endpoint)
        {
            var request = new RestRequest(_variables.DeleteRepoEndpoint + endpoint, Method.Delete);

            request.AddHeader("Accept", _variables.AcceptHeader);
            request.AddHeader("Authorization", _variables.BearerToken);

            return await Client.ExecuteAsync(request);
        }
    }
}
