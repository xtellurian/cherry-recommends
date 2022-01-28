
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
using SignalBox.Core.Recommendations;
using SignalBox.Core.Workflows;

namespace SignalBox.Web.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/hubspotcrmcards/user-features")]
    [Route("api/hubspotcrmcards/customer-metrics")]
    public class HubspotMetricValueCrmCardsController : HubspotConnectorControllerBase
    {
        private readonly ILogger<HubspotMetricValueCrmCardsController> logger;
        private readonly IHasher hasher;
        private readonly ITelemetry telemetry;
        private readonly HubspotWorkflows hubspotWorkflows;
        private readonly IIntegratedSystemStore integratedSystemStore;
        private readonly ITrackedUserSystemMapStore systemMapStore;
        private readonly ITrackedUserTouchpointStore trackedUserTouchpointStore;
        private readonly IParameterSetRecommenderStore parameterSetRecommenderStore;
        private readonly IItemsRecommenderStore itemsRecommenderStore;
        private readonly ParameterSetRecommenderInvokationWorkflows parameterSetRecommenderInvokation;
        private readonly ItemsRecommenderInvokationWorkflows itemsRecommenderInvokation;
        private readonly IMetricStore metricStore;
        private readonly IHistoricCustomerMetricStore customerMetricStore;
        private readonly HubspotAppCredentials credentials;
        private readonly DeploymentInformation deploymentOptions;

        public HubspotMetricValueCrmCardsController(ILogger<HubspotMetricValueCrmCardsController> logger,
                                         IOptions<DeploymentInformation> deploymentOptions,
                                         IHasher hasher,
                                         ITelemetry telemetry,
                                         HubspotWorkflows hubspotWorkflows,
                                         IOptions<HubspotAppCredentials> hubspotOptions,
                                         IIntegratedSystemStore integratedSystemStore,
                                         ITrackedUserSystemMapStore systemMapStore,
                                         ITrackedUserTouchpointStore trackedUserTouchpointStore,
                                         IParameterSetRecommenderStore parameterSetRecommenderStore,
                                         IItemsRecommenderStore itemsRecommenderStore,
                                         ParameterSetRecommenderInvokationWorkflows parameterSetRecommenderInvokation,
                                         ItemsRecommenderInvokationWorkflows itemsRecommenderInvokation,
                                         IMetricStore metricStore,
                                         IHistoricCustomerMetricStore customerMetricStore)
                                          : base(logger, deploymentOptions, hasher, hubspotOptions)
        {
            this.logger = logger;
            this.hasher = hasher;
            this.telemetry = telemetry;
            this.hubspotWorkflows = hubspotWorkflows;
            this.integratedSystemStore = integratedSystemStore;
            this.systemMapStore = systemMapStore;
            this.trackedUserTouchpointStore = trackedUserTouchpointStore;
            this.parameterSetRecommenderStore = parameterSetRecommenderStore;
            this.itemsRecommenderStore = itemsRecommenderStore;
            this.parameterSetRecommenderInvokation = parameterSetRecommenderInvokation;
            this.itemsRecommenderInvokation = itemsRecommenderInvokation;
            this.metricStore = metricStore;
            this.customerMetricStore = customerMetricStore;
            this.credentials = hubspotOptions.Value;
            this.deploymentOptions = deploymentOptions.Value;
        }


        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<HubspotCrmCardResponse> CustomerMetricCrmCard(long id,
                                                          string portalId,
                                                          string userId,
                                                          string userEmail,
                                                          string associatedObjectId,
                                                          string associatedObjectType,
                                                          string objectType)
        {
            await ValidateHubspotSignature(false);
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
                    logger.LogInformation($"Found a Customer {tu.CommonId} for ticket {ticketId}");
                    return await HubspotUserMetricsResponse(tu,
                        integratedSystem.GetCache<HubspotCache>()?.MetricCrmCardBehaviour);
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
                    var customer = await systemMapStore.ReadFromIntegratedSystem(integratedSystem.Id, associatedObjectId);
                    return await HubspotUserMetricsResponse(customer,
                        integratedSystem.GetCache<HubspotCache>()?.MetricCrmCardBehaviour);
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

        private async Task<HubspotCrmCardResponse> HubspotUserMetricsResponse(Customer customer, MetricCrmCardBehaviour behaviour)
        {
            // deal with potential incoming nulls
            behaviour ??= new MetricCrmCardBehaviour();
            behaviour.ExcludedMetrics ??= new HashSet<string>();
            behaviour.IncludedMetrics ??= new HashSet<string>();

            var metrics = await customerMetricStore.GetMetricsFor(customer);
            // filter included by default
            if (behaviour.IncludedMetrics.Any())
            {
                metrics = metrics.Where(_ => behaviour.IncludedMetrics.Contains(_.CommonId));
            }
            else if (behaviour.ExcludedMetrics.Any())
            {
                metrics = metrics.Where(_ => !behaviour.ExcludedMetrics.Contains(_.CommonId));
            }

            var metricValues = new List<HistoricCustomerMetric>();

            var host = Request.Host;
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            if (metrics.Any())
            {
                // var response = new CrmCardResponseWithPrimaryAction<IframeAction>();
                var response = new HubspotCrmCardResponse();
                foreach (var metric in metrics)
                {
                    var val = await customerMetricStore.ReadCustomerMetric(customer, metric);
                    response.AddMetricValueCard(val);
                }

                if (behaviour.HasRecommender())
                {
                    var recommendation = await GetRecommendation(customer, behaviour);
                    response.AddRecommendation(baseUrl, recommendation);
                }

                // not working
                // response.PrimaryAction = new IframeAction
                // {
                //     Height = 600,
                //     Width = 1200,
                //     Label = "View in Four2",
                //     Uri = $"{baseUrl.TrimEnd('/')}/tracked-users/detail/{trackedUser.Id}",
                // };

                return response;
            }
            else
            {
                return DefaultCardResponse($"{customer.Name ?? customer.CommonId} has not been classified.");
            }
        }

        private async Task<RecommendationEntity> GetRecommendation(Customer customer, MetricCrmCardBehaviour behaviour)
        {
            if (behaviour.ParameterSetRecommenderId != null)
            {
                var recommender = await parameterSetRecommenderStore.Read(behaviour.ParameterSetRecommenderId.Value);
                var input = new ParameterSetRecommenderModelInputV1
                {
                    CustomerId = customer.CustomerId
                };
                return await parameterSetRecommenderInvokation.InvokeParameterSetRecommender(recommender, input);
            }
            else if (behaviour.ItemsRecommenderId != null)
            {
                var recommender = await itemsRecommenderStore.Read(behaviour.ItemsRecommenderId.Value);
                var input = new ItemsModelInputDto
                {
                    CustomerId = customer.CustomerId
                };
                return await itemsRecommenderInvokation.InvokeItemsRecommender(recommender, input);
            }
            else
            {
                return new DefaultRecommendation();
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