using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Workflows;

namespace SignalBox.Functions
{
    public class ScheduledFeatureGenerators
    {
        private readonly FeatureGeneratorWorkflows workflows;

        public ScheduledFeatureGenerators(FeatureGeneratorWorkflows workflows)
        {
            this.workflows = workflows;
        }

        [Function("ScheduledFeatureGenerators")]
        // should run once per day at 1300 UTC (approx midnight Australia time)
        public async Task RunScheduled([TimerTrigger("0 0 13 * * * ")] TimerStatus myTimer, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(ScheduledFeatureGenerators));
            logger.LogInformation("Starting to run all feature generators.");
            try
            {
                var totalWrites = await workflows.RunAllFeatureGenerators();
                logger.LogInformation($"Wrote {totalWrites}", new { completedAt = DateTimeOffset.UtcNow });
            }
            catch (System.Exception ex)
            {
                logger.LogError("Error when running all workflows", ex);
                logger.LogError(ex.Message);

            }
        }

        [Function("ManualStartFeatureGenerators")]
        public async Task<HttpResponseData> ManualStart([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
           FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("ManualStartFeatureGenerators");

            logger.LogInformation("Starting to run all feature generators.");
            try
            {
                var totalWrites = await workflows.RunAllFeatureGenerators();
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(new { result = "Ok", totalWrites = totalWrites });

                return response;
            }
            catch (System.Exception ex)
            {
                logger.LogError("Error when running all workflows", ex);
                logger.LogError(ex.Message);
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteAsJsonAsync(new { result = "Error", error = ex.Message, stackTrace = ex.StackTrace });
                response.StatusCode = HttpStatusCode.InternalServerError;
                return response;
            }

        }
    }
}
