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
using SignalBox.Infrastructure.Models;

namespace SignalBox.Functions
{
    public class SegmentEnrolmentRuleScheduler
    {
        private const string SchedulerName = "SegmentEnrolmentRule";
        private readonly ITenantStore tenantStore;
        private readonly Hosting hosting;

        public SegmentEnrolmentRuleScheduler(IOptions<Hosting> hostingOptions, ITenantStore tenantStore)
        {
            this.tenantStore = tenantStore;
            this.hosting = hostingOptions.Value;
        }

        [Function($"Schedule_{SchedulerName}")]
        [QueueOutput(AzureQueueNames.RunAllSegmentEnrolmentRules)]
        // should run once per day at 1600 UTC (approx 3am Australia time)
        public async Task<IEnumerable<RunAllSegmentEnrolmentRulesQueueMessage>> RunScheduler([TimerTrigger("0 0 16 * * * ")] TimerStatus myTimer, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(RunScheduler));
            if (hosting.Multitenant)
            {
                logger.LogInformation("Scheduling all segment enrolment rules.");
                var tenants = await tenantStore.List();

                return tenants.Select(_ => new RunAllSegmentEnrolmentRulesQueueMessage(_.Name));
            }
            else
            {
                logger.LogInformation("Schedulign single tenant segment enrolment rules");
                return new List<RunAllSegmentEnrolmentRulesQueueMessage> { new RunAllSegmentEnrolmentRulesQueueMessage("single") };
            }
        }

        [Function($"Manual_{SchedulerName}")]
        public async Task<ManualScheduleEnrolmentRulesResponse> ManuallySchedule([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(ManuallySchedule));

            if (hosting.Multitenant)
            {
                logger.LogInformation("Scheduling all segment enrolment rules.");
                var tenants = await tenantStore.List();
                var messages = tenants.Select(_ => new RunAllSegmentEnrolmentRulesQueueMessage(_.Name));
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(messages);
                return new ManualScheduleEnrolmentRulesResponse
                {

                    Messages = messages,
                    HttpResponse = response
                };
            }
            else
            {
                var messages = new List<RunAllSegmentEnrolmentRulesQueueMessage> { new RunAllSegmentEnrolmentRulesQueueMessage("single") };
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(messages);
                return new ManualScheduleEnrolmentRulesResponse
                {
                    Messages = messages,
                    HttpResponse = response
                };
            }
        }
    }

    public class ManualScheduleEnrolmentRulesResponse
    {
        [QueueOutput(AzureQueueNames.RunAllSegmentEnrolmentRules)]
        public IEnumerable<RunAllSegmentEnrolmentRulesQueueMessage> Messages { get; set; }
        public HttpResponseData HttpResponse { get; set; }
    }
}
