
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignalBox.Core;
using SignalBox.Core.Integrations;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/hubspotcrmcards")]
    public class HubspotCrmCardsController : SignalBoxControllerBase
    {
        private readonly ILogger<HubspotCrmCardsController> logger;
        private readonly IHasher hasher;
        private readonly IIntegratedSystemStore integratedSystemStore;
        private readonly ITrackedUserSystemMapStore systemMapStore;
        private readonly ITrackedUserTouchpointStore trackedUserTouchpointStore;
        private readonly ITouchpointStore touchpointStore;
        private readonly HubspotAppCredentials credentials;
        private readonly DeploymentInformation deploymentOptions;

        public HubspotCrmCardsController(ILogger<HubspotCrmCardsController> logger,
                                         IOptions<DeploymentInformation> deploymentOptions,
                                         IHasher hasher,
                                         IOptions<HubspotAppCredentials> hubspotOptions,
                                         IIntegratedSystemStore integratedSystemStore,
                                         ITrackedUserSystemMapStore systemMapStore,
                                         ITrackedUserTouchpointStore trackedUserTouchpointStore,
                                         ITouchpointStore touchpointStore)
        {
            this.logger = logger;
            this.hasher = hasher;
            this.integratedSystemStore = integratedSystemStore;
            this.systemMapStore = systemMapStore;
            this.trackedUserTouchpointStore = trackedUserTouchpointStore;
            this.touchpointStore = touchpointStore;
            this.credentials = hubspotOptions.Value;
            this.deploymentOptions = deploymentOptions.Value;
        }


        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<HubspotCrmCardResponse> CrmCard(long id, string portalId, string userId, string userEmail, string associatedObjectId, string objectType)
        {
            // return Generator.GetResponse();
            ValidateHubspotSignature();

            // portal id is actually a number, but we save it as the commonId of the integrated system.
            if (!await integratedSystemStore.ExistsFromCommonId(portalId))
            {
                throw new ConfigurationException($"Hubspot Integrated system with portalId={portalId} does not exist");
            }
            try
            {
                var integratedSystem = await integratedSystemStore.ReadFromCommonId(portalId);
                var allSystemMaps = await systemMapStore.Query(1, (x) => true);
                var systemMaps = await systemMapStore.Query(1,  // first page
                    _ => _.TrackedUser,
                    _ => _.UserId == associatedObjectId && _.IntegratedSystemId == integratedSystem.Id);
                if (systemMaps.Pagination.TotalItemCount == 0)
                {
                    return DefaultCardResponse($"User ({associatedObjectId}) not linked");
                }
                var defaultHubspot = systemMaps.Items.First();

                var touchpoint = await touchpointStore.ReadFromCommonId("Hubspot");

                var touchpointValues = await trackedUserTouchpointStore.ReadTouchpoint(defaultHubspot.TrackedUser, touchpoint);
                // they always need an objectId and title
                touchpointValues.Values["title"] = "Four2 Analysis";
                touchpointValues.Values["objectId"] = touchpointValues.Id;
                return new HubspotCrmCardResponse
                {
                    Results = new List<Dictionary<string, object>>
                    {
                        touchpointValues.Values
                    }
                };
            }
            catch (Exception ex)
            {
                logger.LogCritical("Hubspot CRM Card is failing", ex);
                return DefaultCardResponse("Analysis Pending");
            }
        }

        private HubspotCrmCardResponse DefaultCardResponse(string title) => new HubspotCrmCardResponse
        {
            // the default response. All responses need an objectId and title
            Results = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"title", title},
                    {"objectId", 1}
                }
            }
        };

        private void ValidateHubspotSignature()
        {
            if (Request.Headers.TryGetValue("X-HubSpot-Signature-Version", out var sigVer))
            {
                logger.LogInformation($"X-HubSpot-Signature-Version: {sigVer}");
            }
            else
            {
                logger.LogWarning("X-HubSpot-Signature-Version header found ");
            }
            if (Request.Headers.TryGetValue("X-HubSpot-Signature", out var sig))
            {
                //https://legacydocs.hubspot.com/docs/faq/v2-request-validation
                // var s = "yyyyyyyy-yyyy-yyyy-yyyy-yyyyyyyyyyyyPOSThttps://www.example.com/webhook_uri{\"example_field\":\"サンプルデータ\"}";
                var sigValue = sig.ToString();
                var uri = $"{ Request.Scheme }://{ Request.Host }{ Request.Path }{ Request.QueryString }";
                var s = $"{credentials.ClientSecret}{Request.Method}{uri}";

                var hashedBytes = hasher.HashToBytes(HashingAlgorithms.SHA256, s);
                var hashed = Convert.ToHexString(hashedBytes).ToLower();
                var isValid = string.Equals(hashed, sigValue);

                if (!isValid)
                {
                    throw new BadRequestException("Hubspot Request was not signed correctly.");
                }
                else
                {
                    logger.LogInformation("X-HubSpot-Signature header matches computed value.");
                }
            }
            else
            {
                if (this.deploymentOptions?.Environment?.ToLower() == "production")
                {
                    throw new BadRequestException("X-HubSpot-Signature did not match computed signature");
                }
            }
        }
    }
}