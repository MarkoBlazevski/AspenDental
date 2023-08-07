using AspenDentalApiTask.Setup;
using RestSharp;

namespace AspenDentalApiTask.Clients
{
    public abstract class ApiClient : IApiClient, IDisposable
    {
        protected readonly RestClient Client;
        protected string baseUrl;

        protected ApiClient(string baseUrl)
        {
            this.baseUrl = baseUrl;
            this.Client = new RestClient(baseUrl);
        }

        public string ClientUrlBuilder(string clientBaseUrl, string resource = "")
        {
            return clientBaseUrl + resource;
        }

        public RestRequest RequestBuilder(string endpoint, object parameters = null, object body = null)
        {
            var request = new RestRequest(endpoint);
            if (parameters != null)
            {
                request.AddObject(parameters);
            }
            if (body != null)
            {
                request.AddJsonBody(body);
            }

            return request;
        }

        public async Task<RestResponse> ExecuteRequestAsync(RestRequest request)
        {
            return await Client.ExecuteAsync(request);
        }

        public void Dispose()
        {
            Client?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}