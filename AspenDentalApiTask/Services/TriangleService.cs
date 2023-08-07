using AspenDentalApiTask.Clients;
using AspenDentalApiTask.Models.Responses;
using AspenDentalApiTask.Setup;
using BoDi;
using Newtonsoft.Json;
using System.Net;

namespace AspenDentalApiTask.Services
{
    public class TriangleService
    {
        private readonly TriangleApiClient _triangleApiClient;
        private readonly IObjectContainer _objectContainer;
        public Variables _variables;

        public TriangleService(TriangleApiClient triangleApiClient, IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
            _triangleApiClient = triangleApiClient;
            _variables = _objectContainer.Resolve<Variables>();
        }

        public async Task<(HttpStatusCode ResponseHttp, TriangleResponse ResponseBody)> GetTriangleTypeAsync(object body)
        {
            var response = await _triangleApiClient.PostRequestAsync(_variables.TriangleEndpoint, body);

            // Log the status code and response content
            Console.WriteLine($"Response Status Code: {response.StatusCode}");
            Console.WriteLine($"Response Content: {response.Content}");

            var responseBody = JsonConvert.DeserializeObject<TriangleResponse>(response.Content);

            return (response.StatusCode, responseBody);
        }
    }
}
