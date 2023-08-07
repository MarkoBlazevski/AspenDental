using RestSharp;

namespace AspenDentalApiTask.Setup
{
    public interface IApiClient
    {
        abstract string ClientUrlBuilder(string clientBaseUrl, string resource);
        public RestRequest RequestBuilder(string endpoint, object parameters, object body);
    }
}
