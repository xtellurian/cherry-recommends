using System;
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
    public class FeatureGeneratorScheduler
    {
        private readonly ITenantStore tenantStore;
        private readonly Hosting hosting;

        public FeatureGeneratorScheduler(IOptions<Hosting> hostingOptions, ITenantStore tenantStore)
        {
            this.tenantStore = tenantStore;
            this.hosting = hostingOptions.Value;
        }

        [Function("Schedule_FeatureGeneratorsQueueMessages")]
        [QueueOutput(AzureQueueNames.RunAllFeatureGenerators)]
        // should run once per day at 1300 UTC (approx midnight Australia time)
        public async Task<IEnumerable<RunAllFeatureGeneratorsQueueMessage>> RunScheduler([TimerTrigger("0 0 13 * * * ")] TimerStatus myTimer, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(RunScheduler));
            if (hosting.Multitenant)
            {
                logger.LogInformation("Scheduling all feature generators.");
                var tenants = await tenantStore.List();

                return tenants.Select(_ => new RunAllFeatureGeneratorsQueueMessage(_.Name));
            }
            else
            {
                logger.LogInformation("Schedulign single tenant generators");
                return new List<RunAllFeatureGeneratorsQueueMessage> { new RunAllFeatureGeneratorsQueueMessage("single") };
            }
        }

        [Function("Manual_FeatureGeneratorsQueueMessages")]
        public async Task<ManualScheduleResponse> ManuallySchedule([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(ManuallySchedule));

            if (hosting.Multitenant)
            {
                logger.LogInformation("Scheduling all feature generators.");
                var tenants = await tenantStore.List();
                var messages = tenants.Select(_ => new RunAllFeatureGeneratorsQueueMessage(_.Name));
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(messages);
                return new ManualScheduleResponse
                {

                    Messages = messages,
                    HttpResponse = response
                };
            }
            else
            {
                var messages = new List<RunAllFeatureGeneratorsQueueMessage> { new RunAllFeatureGeneratorsQueueMessage("single") };
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(messages);
                return new ManualScheduleResponse
                {
                    Messages = messages,
                    HttpResponse = response
                };
            }
        }
    }

    public class ManualScheduleResponse
    {
        [QueueOutput(AzureQueueNames.RunAllFeatureGenerators)]
        public IEnumerable<RunAllFeatureGeneratorsQueueMessage> Messages { get; set; }
        public HttpResponseData HttpResponse { get; set; }
    }
}
