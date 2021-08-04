
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Core.Adapters.Hubspot;
using SignalBox.Core.Integrations;
using SignalBox.Core.Integrations.Hubspot;
using SignalBox.Core.Workflows;

namespace SignalBox.Web.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/hubspotcrmcards/user-features")]
    public class HubspotFeatureValueCrmCardsController : HubspotConnectorControllerBase
    {
        private readonly ILogger<HubspotFeatureValueCrmCardsController> logger;
        private readonly IHasher hasher;
        private readonly ITelemetry telemetry;
        private readonly HubspotWorkflows hubspotWorkflows;
        private readonly IIntegratedSystemStore integratedSystemStore;
        private readonly ITrackedUserSystemMapStore systemMapStore;
        private readonly ITrackedUserTouchpointStore trackedUserTouchpointStore;
        private readonly IFeatureStore featureStore;
        private readonly ITrackedUserFeatureStore trackedUserFeatureStore;
        private readonly HubspotAppCredentials credentials;
        private readonly DeploymentInformation deploymentOptions;

        public HubspotFeatureValueCrmCardsController(ILogger<HubspotFeatureValueCrmCardsController> logger,
                                         IOptions<DeploymentInformation> deploymentOptions,
                                         IHasher hasher,
                                         ITelemetry telemetry,
                                         HubspotWorkflows hubspotWorkflows,
                                         IOptions<HubspotAppCredentials> hubspotOptions,
                                         IIntegratedSystemStore integratedSystemStore,
                                         ITrackedUserSystemMapStore systemMapStore,
                                         ITrackedUserTouchpointStore trackedUserTouchpointStore,
                                         IFeatureStore featureStore,
                                         ITrackedUserFeatureStore trackedUserFeatureStore)
                                          : base(logger, deploymentOptions, hasher, hubspotOptions)
        {
            this.logger = logger;
            this.hasher = hasher;
            this.telemetry = telemetry;
            this.hubspotWorkflows = hubspotWorkflows;
            this.integratedSystemStore = integratedSystemStore;
            this.systemMapStore = systemMapStore;
            this.trackedUserTouchpointStore = trackedUserTouchpointStore;
            this.featureStore = featureStore;
            this.trackedUserFeatureStore = trackedUserFeatureStore;
            this.credentials = hubspotOptions.Value;
            this.deploymentOptions = deploymentOptions.Value;
        }


        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<HubspotCrmCardResponse> USerFeatureCrmCard(long id,
                                                          string portalId,
                                                          string userId,
                                                          string userEmail,
                                                          string associatedObjectId,
                                                          string associatedObjectType,
                                                          string objectType)
        {
            await ValidateHubspotSignature();
            telemetry.TrackEvent("Hubspot.CrmCard", new Dictionary<string, string>
            {
                {"portalId", portalId},
                {"userId", userId},
                {"userEmail", userEmail},
                {"associatedObjectId", associatedObjectId},
                {"associatedObjectType", associatedObjectType},
                {"objectType", objectType},
            });

            // portal id is actually a number, but we save it as the commonId of the integrated system.
            if (!await integratedSystemStore.ExistsFromCommonId(portalId))
            {
                throw new ConfigurationException($"Hubspot Integrated s ystem with portalId={portalId} does not exist");
            }

            switch (associatedObjectType)
            {
                case "CONTACT":
                    return await HandleContact(portalId, associatedObjectId);
                case "TICKET":
                    return await HandleTicket(portalId, associatedObjectId);
                default:
                    return DefaultCardResponse($"Unknown Object Type: {associatedObjectType}");
            }
        }

        private async Task<HubspotCrmCardResponse> HandleTicket(string portalId, string ticketId)
        {
            try
            {
                var integratedSystem = await integratedSystemStore.ReadFromCommonId(portalId);
                var trackedUsers = await hubspotWorkflows.GetAssociatedTrackedUsersFromTicket(portalId, ticketId);
                if (trackedUsers.Any())
                {
                    var tu = trackedUsers.First();
                    logger.LogInformation($"Found a TrackedUser {tu.CommonId} for ticket {ticketId}");
                    return await HubspotUserFeaturesResponse(tu,
                        integratedSystem.GetCache<HubspotCache>()?.FeatureCrmCardBehaviour?.ExcludedFeatures);
                }
                else
                {
                    logger.LogWarning($"No tracked users associated with ticket {ticketId}");
                    return DefaultCardResponse("No Tracked User linked to this ticket.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to access Hubspot analysis for ticketId {ticketId}", ex);
                return DefaultCardResponse("Analysis unavailable");
            }
        }
        private async Task<HubspotCrmCardResponse> HandleContact(string portalId, string associatedObjectId)
        {
            try
            {
                var integratedSystem = await integratedSystemStore.ReadFromCommonId(portalId);
                if (await systemMapStore.ExistsInIntegratedSystem(integratedSystem.Id, associatedObjectId))
                {
                    var trackedUser = await systemMapStore.ReadFromIntegratedSystem(integratedSystem.Id, associatedObjectId);
                    return await HubspotUserFeaturesResponse(trackedUser,
                        integratedSystem.GetCache<HubspotCache>()?.FeatureCrmCardBehaviour?.ExcludedFeatures);
                }
                else
                {
                    return DefaultCardResponse($"User ({associatedObjectId}) not linked");
                }
            }
            catch (SignalBox.Core.StorageException ex)
            {
                logger.LogCritical("Hubspot CRM Card is failing", ex);
                return DefaultCardResponse("Analysis Process Running");
            }
            catch (Exception ex)
            {
                logger.LogCritical("Hubspot CRM Card is failing", ex);
                return DefaultCardResponse("Analysis Pending");
            }
        }

        private async Task<HubspotCrmCardResponse> HubspotUserFeaturesResponse(TrackedUser trackedUser, IEnumerable<string> excludedFeatures = null)
        {
            excludedFeatures ??= new List<string>();
            var features = await trackedUserFeatureStore.GetFeaturesFor(trackedUser);
            features = features.Where(_ => !excludedFeatures.Contains(_.CommonId));
            var featureValues = new List<TrackedUserFeature>();

            if (features.Any())
            {
                var response = new HubspotCrmCardResponse();
                foreach (var feature in features)
                {
                    var val = await trackedUserFeatureStore.ReadFeature(trackedUser, feature);
                    response.AddFeatureValueCard(val);
                }

                return response;
            }
            else
            {
                return DefaultCardResponse($"{trackedUser.Name ?? trackedUser.CommonId} has not been classified.");
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
    }
}