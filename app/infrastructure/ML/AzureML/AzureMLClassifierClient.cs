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
    public class AzureMLClassifierClient : TabularClassifier
    {
        private JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private readonly HttpClient httpClient;

        public AzureMLClassifierClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public override async Task<EvaluationResult> Invoke(ModelRegistration model, IDictionary<string, object> features)
        {
            // convert to strings - Azure seems to want it that way?
            var res = await RequestScore(model, features.ToDictionary(pair => pair.Key, pair => pair.Value.ToString()));
            return ParseResults(res);
        }

        private EvaluationResult ParseResults(string results)
        {
            // Azure ML does a stupid thing where it doesn't serialise JSON properly
            var deStringed = JsonSerializer.Deserialize<string>(results);
            var r = JsonSerializer.Deserialize<AzureMLClassificationResponse>(deStringed, serializerOptions);
            return new EvaluationResult(r.Result.FirstOrDefault());
        }

        private async Task<string> RequestScore(ModelRegistration model, params IDictionary<string, string>[] features)
        {
            var scoringPayload = new Dictionary<string, List<IDictionary<string, string>>>();
            scoringPayload["data"] = features.ToList();

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.Key);
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await httpClient.PostAsJsonAsync(model.ScoringUrl, scoringPayload);
            // response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}