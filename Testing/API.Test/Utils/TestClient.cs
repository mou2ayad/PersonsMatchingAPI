using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;

namespace API.Test.Utils
{
    public class TestClient
    {
        private readonly HttpClient _httpClient;
        private const string BasePath = "https://localhost:5001/";
        public TestServer WebServer { get; }

        public TestClient(TestServer server)
        {
            WebServer = server;
            _httpClient = server.CreateClient();
        } 
        
        private void AddHeaders(params HttpTestHeader[] headers)
        {
            if(headers!=null)
                foreach (var header in headers)
                    _httpClient.DefaultRequestHeaders.Add(header.Name, header.Value);
        }

        public async Task<HttpResponseMessage> Post(string uri, object request, params HttpTestHeader[] headers)
        {
            AddHeaders(headers);
            return await _httpClient.PostAsync(BuildUri(uri), Serialize(request));
        }

        public async Task<T> Post<T>(string uri, object request, params HttpTestHeader[] headers)
        {
            AddHeaders(headers);
            var result = await _httpClient.PostAsync(BuildUri(uri), Serialize(request));
            return await result.Content.ReadFromJsonAsync<T>();

        }

        private static string BuildUri(string uri) => $"{BasePath}{uri}";

        private static StringContent Serialize(object request) => new(JsonConvert.SerializeObject(request),
            Encoding.UTF8, "application/json");
        
    }
}