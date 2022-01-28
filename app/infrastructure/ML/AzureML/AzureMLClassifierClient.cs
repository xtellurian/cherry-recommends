using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure.ML.Azure
{
    public class AzureMLClassifierClient : MLModelClient, IModelClient<AzureMLModelInput, AzureMLClassifierOutput>
    {
        private readonly HttpClient httpClient;

        public AzureMLClassifierClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            base.SetApplicationJsonHeader(this.httpClient);
        }

        public async Task<AzureMLClassifierOutput> Invoke(ModelRegistration model, string version, AzureMLModelInput input)
        {
            // convert to strings - Azure seems to want it that way?
            var res = await RequestScore(model, input.Data.First().ToDictionary(pair => pair.Key, pair => pair.Value.ToString()));
            return ParseResults(res);
        }

        private AzureMLClassifierOutput ParseResults(string results)
        {
            // Azure ML does a stupid thing where it doesn't serialise JSON properly
            var deStringed = JsonSerializer.Deserialize<string>(results);
            var r = JsonSerializer.Deserialize<AzureMLClassificationResponse>(deStringed, serializerOptions);
            return new AzureMLClassifierOutput(r.Result.FirstOrDefault());
        }

        private async Task<string> RequestScore(ModelRegistration model, params IDictionary<string, string>[] metrics)
        {
            var scoringPayload = new Dictionary<string, List<IDictionary<string, string>>>();
            scoringPayload["data"] = metrics.ToList();
            SetKeyAsBearerToken(httpClient, model);
            var response = await httpClient.PostAsJsonAsync(model.ScoringUrl, scoringPayload);
            // response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}