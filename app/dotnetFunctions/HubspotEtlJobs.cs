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
using SignalBox.Core.Workflows;
using SignalBox.Infrastructure.Models;

namespace SignalBox.Functions
{
    public class RunHubspotEtlJobs
    {
        private const string SchedulerName = "HubspotEtlJobs";
        private const string JobName = "HubspotEtl";
        private readonly ITenantStore tenantStore;
        private readonly Hosting hosting;
        private readonly HubspotEtlWorkflows workflows;
        private readonly IIntegratedSystemStore integratedSystemStore;

        public RunHubspotEtlJobs(HubspotEtlWorkflows workflows,
                                IIntegratedSystemStore integratedSystemStore,
                                IOptions<Hosting> hostingOptions,
                                ITenantStore tenantStore)
        {
            this.workflows = workflows;
            this.integratedSystemStore = integratedSystemStore;
            this.tenantStore = tenantStore;
            this.hosting = hostingOptions.Value;
        }

        [Function($"Schedule_{SchedulerName}")]
        [QueueOutput(AzureQueueNames.RunAllHubspotEtl)]
        // should run once per day at 1600 UTC SUNDAY (approx 2 am Australia time)
        public async Task<IEnumerable<RunAllHubspotEtlQueueMessage>> RunFromTimer([TimerTrigger("0 0 15 * * * ")] TimerStatus timer, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(RunFromTimer));
            if (hosting.Multitenant)
            {
                logger.LogInformation($"Starting scheduled Hubspot ETL, next is at: {timer?.ScheduleStatus?.Next}");
                var tenants = await tenantStore.List();

                return tenants.Select(_ => new RunAllHubspotEtlQueueMessage(_.Name));
            }
            else
            {
                logger.LogInformation("Scheduling single tenant Hubspot ETL");
                return new List<RunAllHubspotEtlQueueMessage> { new RunAllHubspotEtlQueueMessage("single") };
            }
        }

        [Function($"Run__{JobName}_FanOutForTenant")]
        [QueueOutput(AzureQueueNames.RunHubspotEtl)]
        public async Task<IEnumerable<RunHubspotEtlQueueMessage>> FanOutHubspotEtl(
           [QueueTrigger(AzureQueueNames.RunAllHubspotEtl)] RunAllHubspotEtlQueueMessage message, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(FanOutHubspotEtl));
            logger.LogInformation("Fanning out for job {jobName}", JobName);

            var systems = new List<IntegratedSystem>();
            await foreach (var system in integratedSystemStore.Iterate(
                new EntityStoreIterateOptions<IntegratedSystem>(_ => _.SystemType == IntegratedSystemTypes.Hubspot)))
            {
                systems.Add(system);
            }

            return systems.Select(_ => new RunHubspotEtlQueueMessage(message.TenantName, _.Id, _.EnvironmentId));
        }

        [Function($"Run_{JobName}")]
        public async Task RunHubspotEtlFromQueue([QueueTrigger(AzureQueueNames.RunHubspotEtl)] RunHubspotEtlQueueMessage message, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(RunHubspotEtlFromQueue));
            logger.LogInformation($"Running job {JobName}");

            var integratedSystem = await integratedSystemStore.Read(message.IntegratedSystemId);
            var result = await workflows.RunHubspotContactEtlJob(integratedSystem);
            logger.LogInformation($"Updated {result.NumberOfTrackedUsersUpdated} in system common Id = {result.HubspotSystemCommonId}");
        }

        [Function("RunHubspotEtlJobs")]
        public async Task<HttpResponseData> RunFromHttp([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {

            var logger = executionContext.GetLogger("RunHubspotEtlJobs");
            List<HubspotEtlReport> reports = await RunJob(logger);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(reports);

            return response;
        }

        private async Task<List<HubspotEtlReport>> RunJob(ILogger logger)
        {
            var reports = new List<HubspotEtlReport>();
            await foreach (var s in integratedSystemStore.Iterate(
                new EntityStoreIterateOptions<IntegratedSystem>(_ => _.SystemType == IntegratedSystemTypes.Hubspot)))
            {
                logger.LogInformation($"Starting ETL for hubspot, portal ID (commonId) = {s.CommonId}");
                var result = await workflows.RunHubspotContactEtlJob(s);
                reports.Add(result);
                logger.LogInformation($"Updated {result.NumberOfTrackedUsersUpdated} in system common Id = ${result.HubspotSystemCommonId}");
            }

            return reports;
        }
    }
}
