using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Workflows;

namespace SignalBox.Functions
{
    public class RunHubspotEtlJobs
    {
        private readonly HubspotEtlWorkflows workflows;
        private readonly IIntegratedSystemStore integratedSystemStore;

        public RunHubspotEtlJobs(HubspotEtlWorkflows workflows, IIntegratedSystemStore integratedSystemStore)
        {
            this.workflows = workflows;
            this.integratedSystemStore = integratedSystemStore;
        }

        // [Function("ScheduledHubspotEtlJobs")]
        // TODO: Fix in multitenant hubspot
        // should run once per day at 1500 UTC (approx 2 am Australia time)
        public async Task RunFromTimer([TimerTrigger("0 0 15 * * * ")] TimerStatus timer, FunctionContext context)

        {
            var logger = context.GetLogger("ScheduledHubspotEtlJobs");
            logger.LogInformation($"Starting scheduled Hubspot ETL, next is at: {timer?.ScheduleStatus?.Next}");
            await RunJob(logger);
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
            await foreach (var s in integratedSystemStore.Iterate(_ => _.SystemType == IntegratedSystemTypes.Hubspot))
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
