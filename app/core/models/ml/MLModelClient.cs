using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SignalBox.Core
{
    public abstract class MLModelClient
    {
        protected JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        protected void SetApplicationJsonHeader(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        protected void SetKeyAsBearerToken(HttpClient httpClient, ModelRegistration model)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.Key);
        }

    }
}