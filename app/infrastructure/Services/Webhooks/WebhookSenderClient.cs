using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommendations.Destinations;
using SignalBox.Infrastructure.Dto;
#nullable enable
namespace SignalBox.Infrastructure.Webhooks
{
    public class WebhookSenderClient : IWebhookSenderClient
    {
        private readonly HttpClient httpClient;
        private readonly IHasher hasher;
        private readonly IIntegratedSystemStore systemStore;
        private readonly ILogger<WebhookSenderClient> logger;
        private readonly JsonSerializerOptions jOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private const string SignatureHeaderName = "X-Four2-Signature"; // this is public API

        public WebhookSenderClient(HttpClient httpClient,
                                   IHasher hasher,
                                   IIntegratedSystemStore systemStore,
                                   ILogger<WebhookSenderClient> logger)
        {
            this.httpClient = ConfigureHttpClient(httpClient);
            this.hasher = hasher;
            this.systemStore = systemStore;
            this.logger = logger;
        }

        private static HttpClient ConfigureHttpClient(HttpClient httpClient)
        {
            httpClient.Timeout = new System.TimeSpan(0, 0, 5); // 5 seconds

            return httpClient;
        }

        private string Serialize<T>(T dto) where T : IRecommendationDto
        {
            return JsonSerializer.Serialize(dto, jOptions);
        }

        public async Task Send<TRecommendation>(WebhookDestination destination, TRecommendation recommendation)
        where TRecommendation : RecommendationEntity
        {
            var connectedSystem = destination.ConnectedSystem;

            if (connectedSystem == null)
            {
                connectedSystem = await systemStore.Read(destination.ConnectedSystemId);
            }
            if (connectedSystem == null)
            {
                throw new WorkflowException($"Cannot find connected integrated system id = {destination.ConnectedSystemId}");
            }

            if (recommendation is ParameterSetRecommendation psRec)
            {
                var parameters = recommendation.GetOutput<ParameterSetRecommenderModelOutputV1>().RecommendedParameters;
                var serialized = Serialize(new ParameterSetRecommendationDto(psRec, parameters));
                await SendStringWithSig(destination, serialized);
            }
            else if (recommendation is ItemsRecommendation itemRec)
            {
                var serialized = Serialize(new PromotionsRecommendationDto(itemRec));
                await SendStringWithSig(destination, serialized);
            }
            else
            {
                throw new ConfigurationException("Unknown type of recommendation to send");
            }
        }

        public async Task Send<TRecommendation>(SegmentSourceFunctionDestination destination, TRecommendation recommendation) where TRecommendation : RecommendationEntity
        {
            var connectedSystem = destination.ConnectedSystem;
            if (connectedSystem == null)
            {
                _ = await systemStore.Read(destination.ConnectedSystemId);
            }

            if (recommendation is ParameterSetRecommendation psRec)
            {
                var parameters = recommendation.GetOutput<ParameterSetRecommenderModelOutputV1>().RecommendedParameters;
                var dto = new ParameterSetRecommendationDto(psRec, parameters);
                var serialized = Serialize(dto);
                await HttpPostToDestination(destination, serialized);
            }
            else if (recommendation is ItemsRecommendation itemRec)
            {
                var dto = new PromotionsRecommendationDto(itemRec);
                var serialized = Serialize(dto);
                await HttpPostToDestination(destination, serialized);
            }
            else
            {
                throw new ConfigurationException("Unknown type of recommendation to send");
            }
        }

        public async Task Send<TPayload>(IWebhookDestination destination, TPayload payload)
        {
            await SendStringWithSig(destination, JsonSerializer.Serialize(payload, jOptions));
        }

        private async Task SendStringWithSig(IWebhookDestination destination, string serialized)
        {
            var sig = hasher.HashHttpRequestForWebhookValidation(destination.ApplicationSecret, serialized);
            await HttpPostToDestination(destination, serialized, sig);
        }

        private async Task HttpPostToDestination(IWebhookDestination destination, string serialized, string? sigHeader = null)
        {
            logger.LogInformation("Posting to destination {endpoint}", destination.Endpoint);
            try
            {
                var content = new StringContent(serialized, System.Text.Encoding.UTF8, "application/json");
                if (sigHeader != null)
                {
                    content.Headers.Add(SignatureHeaderName, sigHeader);
                }
                await httpClient.PostAsync(destination.Endpoint, content);
            }
            catch (System.Exception ex)
            {
                logger.LogError("Error sending to endpoint: message: {message}", ex.Message);
                throw new RecommenderInvokationException("Error sending to endpoint", ex.Message, ex);
            }
        }
    }
}