using AspenDentalApiTask.Setup;
using BoDi;
using RestSharp;

namespace AspenDentalApiTask.Clients
{
    public class TriangleApiClient : ApiClient
    {
        public Variables _variables;
        readonly IObjectContainer _objectContainer;

        public TriangleApiClient(IObjectContainer objectContainer)
            : base(objectContainer.Resolve<Variables>().TriangleApiUrl)
        {
            _objectContainer = objectContainer;
            _variables = _objectContainer.Resolve<Variables>();
        }

        public async Task<RestResponse> PostRequestAsync(string endpoint, object body)
        {
            var request = new RestRequest(baseUrl + endpoint, Method.Post);

            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(body);

            return await Client.ExecuteAsync(request);
        }
    }
}
