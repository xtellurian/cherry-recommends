using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Core.Constants;
using SignalBox.Core.Integrations.Hubspot;
using SignalBox.Core.Workflows;
using SignalBox.Infrastructure.Models;

namespace SignalBox.Functions
{
    public class HubspotDataPushJobs
    {
        private const string SchedulerName = "HubspotDataPushJobs";
        private const string JobName = "HubspotDataPush";
        private readonly ITenantStore tenantStore;
        private readonly Hosting hosting;
        private readonly HubspotPushWorkflows workflows;
        private readonly IPromotionsCampaignStore itemsRecommenderStore;
        private readonly IIntegratedSystemStore integratedSystemStore;

        public HubspotDataPushJobs(HubspotPushWorkflows workflows,
                                   IPromotionsCampaignStore itemsRecommenderStore,
                                   IIntegratedSystemStore integratedSystemStore,
                                   IOptions<Hosting> hostingOptions,
                                   ITenantStore tenantStore)
        {
            this.workflows = workflows;
            this.itemsRecommenderStore = itemsRecommenderStore;
            this.integratedSystemStore = integratedSystemStore;
            this.tenantStore = tenantStore;
            this.hosting = hostingOptions.Value;
        }

        [Function($"Schedule_{SchedulerName}")]
        [QueueOutput(AzureQueueNames.RunAllHubspotDataPush)]
        // should run once per day at 1600 UTC SUNDAY (approx 2 am Australia time)
        public async Task<IEnumerable<RunAllHubspotDataPushQueueMessage>> RunFromTimer([TimerTrigger("0 0 16 * * 0")] TimerStatus timer, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(RunFromTimer));
            if (hosting.Multitenant)
            {
                logger.LogInformation($"Starting scheduled Hubspot Data Push, next is at: {timer?.ScheduleStatus?.Next}");
                var tenants = await tenantStore.List();

                return tenants.Select(_ => new RunAllHubspotDataPushQueueMessage(_.Name));
            }
            else
            {
                logger.LogInformation("Scheduling single tenant Hubspot Data Push");
                return new List<RunAllHubspotDataPushQueueMessage> { new RunAllHubspotDataPushQueueMessage("single") };
            }
        }

        [Function($"Run__{JobName}_FanOutForTenant")]
        [QueueOutput(AzureQueueNames.RunHubspotDataPush)]
        public async Task<IEnumerable<RunHubspotDataPushQueueMessage>> FanOutHubspotDataPush(
           [QueueTrigger(AzureQueueNames.RunAllHubspotDataPush)] RunAllHubspotDataPushQueueMessage message, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(FanOutHubspotDataPush));
            logger.LogInformation("Fanning out for job {jobName}", JobName);

            var systems = new List<IntegratedSystem>();
            await foreach (var system in integratedSystemStore.Iterate(
                new EntityStoreIterateOptions<IntegratedSystem>(_ => _.SystemType == IntegratedSystemTypes.Hubspot)))
            {
                systems.Add(system);
            }

            return systems.Select(_ => new RunHubspotDataPushQueueMessage(message.TenantName, _.Id, _.EnvironmentId));
        }

        [Function($"Run_{JobName}")]
        public async Task RunHubspotDataPushFromQueue([QueueTrigger(AzureQueueNames.RunHubspotDataPush)] RunHubspotDataPushQueueMessage message, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(RunHubspotDataPushFromQueue));
            logger.LogInformation($"Running job {JobName}");

            var integratedSystem = await integratedSystemStore.Read(message.IntegratedSystemId);
            await RunHubspotDataPushJob(integratedSystem, logger);
        }

        [Function("RunHubspotDataPush")]
        public async Task<HttpResponseData> RunFromHttp([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            try
            {
                var logger = executionContext.GetLogger("RunHubspotDataPush");
                var reports = await RunJob(logger);
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(reports);
                return response;
            }
            catch (IntegratedSystemException sysEx)
            {
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteStringAsync($"Title: {sysEx.Title} , Message: {sysEx.Message}");
                return response;
            }
            catch (System.Exception ex)
            {
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteStringAsync($"Message: {ex.Message}");
                return response;
            }
        }

        private async Task<List<HubspotDataPushReport>> RunJob(ILogger logger)
        {
            var reports = new List<HubspotDataPushReport>();
            await foreach (var s in integratedSystemStore.Iterate(
                new EntityStoreIterateOptions<IntegratedSystem>(_ => _.SystemType == IntegratedSystemTypes.Hubspot)))
            {
                var perSytemReports = await RunHubspotDataPushJob(s, logger);
                reports.AddRange(perSytemReports);
            }

            return reports;
        }

        private async Task<List<HubspotDataPushReport>> RunHubspotDataPushJob(IntegratedSystem integratedSystem, ILogger logger)
        {
            if (integratedSystem.SystemType != IntegratedSystemTypes.Hubspot)
            {
                logger.LogWarning($"Unknown system type for Hubspot Data Push: {integratedSystem.SystemType}");
                return new List<HubspotDataPushReport>();
            }

            var reports = new List<HubspotDataPushReport>();
            logger.LogInformation($"Starting Data Push for hubspot, portal ID (commonId) = {integratedSystem.CommonId}");
            var cache = integratedSystem.GetCache<HubspotCache>();
            foreach (var rId in cache?.PushBehaviour?.RecommenderIds ?? new HashSet<long>())
            {
                var recommender = await itemsRecommenderStore.Read(rId);
                var report = await workflows.RecommendForAllHubspotContacts(integratedSystem, recommender);
                reports.Add(report);
                logger.LogInformation($"Updated {report.NumberOfContactsUpdated} contacts Hubspot = {report.HubspotSystemCommonId}");
            }
            if (cache?.PushBehaviour?.RecommenderIds?.Any() == false)
            {
                logger.LogInformation($"Skipping system {integratedSystem.Id} - it has no recommenders to push");
            }

            return reports;
        }
    }
}
