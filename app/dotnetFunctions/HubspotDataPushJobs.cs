using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Integrations.Hubspot;
using SignalBox.Core.Workflows;

namespace SignalBox.Functions
{
    public class HubspotDataPushJobs
    {
        private readonly HubspotPushWorkflows workflows;
        private readonly IItemsRecommenderStore itemsRecommenderStore;
        private readonly IIntegratedSystemStore integratedSystemStore;

        public HubspotDataPushJobs(HubspotPushWorkflows workflows,
                                   IItemsRecommenderStore itemsRecommenderStore,
                                   IIntegratedSystemStore integratedSystemStore)
        {
            this.workflows = workflows;
            this.itemsRecommenderStore = itemsRecommenderStore;
            this.integratedSystemStore = integratedSystemStore;
        }

        [Function("ScheduledHubspotDataPushJobs")]
        // should run once per day at 1600 UTC SUNDAY (approx 2 am Australia time)
        public async Task RunFromTimer([TimerTrigger("0 0 16 * * 0 ")] TimerStatus timer, FunctionContext context)

        {
            var logger = context.GetLogger("ScheduledHubspotDataPushJobs");
            logger.LogInformation($"Starting scheduled Hubspot Data Push, next is at: {timer?.ScheduleStatus?.Next}");
            await RunJob(logger);
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
            var systems = await integratedSystemStore.Query(1, _ => _.SystemType == IntegratedSystemTypes.Hubspot);
            var reports = new List<HubspotDataPushReport>();
            foreach (var s in systems.Items)
            {
                logger.LogInformation($"Starting Data Push for hubspot, portal ID (commonId) = {s.CommonId}");
                var cache = s.GetCache<HubspotCache>();
                foreach (var rId in cache?.PushBehaviour?.RecommenderIds ?? new HashSet<long>())
                {
                    var recommender = await itemsRecommenderStore.Read(rId);
                    var report = await workflows.RecommendForAllHubspotContacts(s, recommender);
                    reports.Add(report);
                    logger.LogInformation($"Updated {report.NumberOfContactsUpdated} contacts Hubspot = ${report.HubspotSystemCommonId}");
                }
                if (cache?.PushBehaviour?.RecommenderIds?.Any() == false)
                {
                    logger.LogInformation($"Skipping system {s.Id} - it has no recommenders to push");
                }
            }

            return reports;
        }
    }
}
