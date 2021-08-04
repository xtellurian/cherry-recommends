
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
using SignalBox.Core.Workflows;

namespace SignalBox.Web.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/hubspotcrmcards")]
    public class HubspotCrmCardsController : HubspotConnectorControllerBase
    {
        private readonly ILogger<HubspotCrmCardsController> logger;
        private readonly IHasher hasher;
        private readonly ITelemetry telemetry;
        private readonly HubspotWorkflows hubspotWorkflows;
        private readonly IIntegratedSystemStore integratedSystemStore;
        private readonly ITrackedUserSystemMapStore systemMapStore;
        private readonly ITrackedUserTouchpointStore trackedUserTouchpointStore;
        private readonly ITouchpointStore touchpointStore;
        private readonly HubspotAppCredentials credentials;
        private readonly DeploymentInformation deploymentOptions;

        public HubspotCrmCardsController(ILogger<HubspotCrmCardsController> logger,
                                         IOptions<DeploymentInformation> deploymentOptions,
                                         IHasher hasher,
                                         ITelemetry telemetry,
                                         HubspotWorkflows hubspotWorkflows,
                                         IOptions<HubspotAppCredentials> hubspotOptions,
                                         IIntegratedSystemStore integratedSystemStore,
                                         ITrackedUserSystemMapStore systemMapStore,
                                         ITrackedUserTouchpointStore trackedUserTouchpointStore,
                                         ITouchpointStore touchpointStore) : base(logger, deploymentOptions, hasher, hubspotOptions)
        {
            this.logger = logger;
            this.hasher = hasher;
            this.telemetry = telemetry;
            this.hubspotWorkflows = hubspotWorkflows;
            this.integratedSystemStore = integratedSystemStore;
            this.systemMapStore = systemMapStore;
            this.trackedUserTouchpointStore = trackedUserTouchpointStore;
            this.touchpointStore = touchpointStore;
            this.credentials = hubspotOptions.Value;
            this.deploymentOptions = deploymentOptions.Value;
        }


        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<HubspotCrmCardResponse> CrmCard(long id,
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
                var trackedUsers = await hubspotWorkflows.GetAssociatedTrackedUsersFromTicket(portalId, ticketId);
                if (trackedUsers.Any())
                {
                    var tu = trackedUsers.First();
                    logger.LogInformation($"Found a TrackedUser {tu.CommonId} for ticket {ticketId}");
                    return await HubspotTouchpointResponse(tu);
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
                // var allSystemMaps = await systemMapStore.Query(1, (x) => true);
                var systemMaps = await systemMapStore.Query(1,  // first page
                    _ => _.TrackedUser,
                    _ => _.UserId == associatedObjectId && _.IntegratedSystemId == integratedSystem.Id);
                if (systemMaps.Pagination.TotalItemCount == 0)
                {
                    return DefaultCardResponse($"User ({associatedObjectId}) not linked");
                }
                var trackedUser = systemMaps.Items.First().TrackedUser;
                return await HubspotTouchpointResponse(trackedUser);
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

        private async Task<HubspotCrmCardResponse> HubspotTouchpointResponse(TrackedUser trackedUser)
        {
            var touchpoint = await touchpointStore.ReadFromCommonId("Hubspot");
            if (await trackedUserTouchpointStore.TouchpointExists(trackedUser, touchpoint))
            {
                var touchpointValues = await trackedUserTouchpointStore.ReadTouchpoint(trackedUser, touchpoint);
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
            else
            {
                return DefaultCardResponse($"{trackedUser.Name ?? trackedUser.CommonId} analysis pending.");
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