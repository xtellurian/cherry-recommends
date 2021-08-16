using System.Collections.Generic;
using System.Text.Json;
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
    [Route("api/hubspotwebhooks")]
    public class HubspotWebhooksController : HubspotConnectorControllerBase
    {
        private readonly ILogger<HubspotWebhooksController> logger;
        private readonly IHasher hasher;
        private readonly ITelemetry telemetry;
        private readonly HubspotWorkflows hubspotWorkflows;
        private readonly IIntegratedSystemStore integratedSystemStore;
        private readonly ITrackedUserSystemMapStore systemMapStore;
        private readonly ITrackedUserTouchpointStore trackedUserTouchpointStore;
        private readonly ITouchpointStore touchpointStore;
        private readonly HubspotAppCredentials credentials;
        private readonly DeploymentInformation deploymentOptions;

        public HubspotWebhooksController(ILogger<HubspotWebhooksController> logger,
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


        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task ProcessWebhook()
        {
            var content = await Request.GetRawBodyStringAsync();
            await ValidateHubspotSignature(true, content);

            var payloads = JsonSerializer.Deserialize<List<HubspotWebhookPayload>>(content);
            telemetry.TrackEvent("Hubspot.Webhook");

            foreach (var p in payloads)
            {
                await HandleWebhookPayload(p);
            }
        }

        // {?userId=25089465&userEmail=rian@four2.ai&associatedObjectId=51&associatedObjectType=CONTACT&portalId=20210647}
        [HttpGet("recommendations/{correlationId}/outcomes/{outcome}")] // use correlation ID for now but potentially change it?
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<HubspotActionHookResponse> ProcessActionHook(long correlationId,
                                                                        string outcome,
                                                                       string userId,
                                                                       string userEmail,
                                                                       string associatedObjectId,
                                                                       string associatedObjectType,
                                                                       string portalId)
        {
            // everything is in the request URI (no body)
            var requestBody = await Request.GetRawBodyStringAsync();
            await ValidateHubspotSignature(false);
            var integratedSystem = await integratedSystemStore.ReadFromCommonId(portalId);
            var response = await hubspotWorkflows.HandleHubspotRecommendationOutcome(integratedSystem, correlationId, outcome, userId, userEmail, associatedObjectId, associatedObjectType);

            return new HubspotActionHookResponse($"{outcome} logged. {response.EventsProcessed} events processed");
        }

        private async Task HandleWebhookPayload(HubspotWebhookPayload payload)
        {
            var portalId = payload.PortalId.ToString();
            if (!await integratedSystemStore.ExistsFromCommonId(portalId))
            {
                throw new ConfigurationException($"Hubspot Integrated s ystem with portalId={portalId} does not exist");
            }
            var integratedSystem = await integratedSystemStore.ReadFromCommonId(portalId);
            try
            {
                await hubspotWorkflows.HandleWebhookPayload(integratedSystem, payload);
            }
            catch (WorkflowCancelledException cancelledException)
            {
                logger.LogInformation($"Webhook workflow was cancelled. {cancelledException.Message}");
            }
        }
    }
}