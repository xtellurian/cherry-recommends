using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Adapters.Klaviyo;
using SignalBox.Core.Integrations;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Serialization;

namespace SignalBox.Infrastructure.Services
{
    public class KlaviyoService : IKlaviyoService
    {
        private const string BaseUrl = "https://a.klaviyo.com/api";
        private readonly HttpClient httpClient;
        private readonly ILogger<KlaviyoService> logger;

        public KlaviyoService(HttpClient httpClient, ILogger<KlaviyoService> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<IEnumerable<KlaviyoList>> GetLists(KlaviyoApiKeys apiKeys)
        {
            var listEndpoint = $"{BaseUrl}/v2/lists?api_key={apiKeys.PrivateKey}";
            var response = await httpClient.GetAsync(listEndpoint);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var klaviyoLists = Serializer.Deserialize<List<KlaviyoList>>(content);

            return klaviyoLists;
        }

        public async Task<IEnumerable<KlaviyoProfileResponse>> SubscribeCustomerToList<TRecommendation>(KlaviyoApiKeys apiKeys, string listTriggerId, TRecommendation recommendation) where TRecommendation : RecommendationEntity
        {
            if (string.IsNullOrEmpty(recommendation.Customer?.Email))
            {
                throw new BadRequestException("Email address is required for Klaviyo");
            }

            var subscribeEndpoint = $"{BaseUrl}/v2/list/{listTriggerId}/subscribe?api_key={apiKeys.PrivateKey}";

            KlaviyoProfileCollection profileList = new KlaviyoProfileCollection();
            if (recommendation is ItemsRecommendation itemRecommendation)
            {
                // create Klaviyo subscription request with promotion data
                var newProfile = new KlaviyoProfileRequest(itemRecommendation);
                profileList.Profiles.Add(newProfile);
            }

            try
            {
                string jsonString = Serializer.Serialize(profileList);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                // klaviyo profile will only be created if person does not exist yet, otherwise properties will just be updated
                var response = await httpClient.PostAsync(subscribeEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();

                var respProfiles = Serializer.Deserialize<List<KlaviyoProfileResponse>>(responseContent);
                return respProfiles;
            }
            catch (Exception ex)
            {
                logger.LogError("Error subscribing profile to Klaviyo", ex.Message);
                throw new WorkflowException("Error subscribing profile to Klaviyo", ex);
            }
        }
    }
}

