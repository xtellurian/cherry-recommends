using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Constants;
using SignalBox.Core.Workflows;

namespace SignalBox.Functions
{
    public class ScheduledFeatureGenerators
    {
        private readonly FeatureGeneratorWorkflows workflows;
        private readonly IFeatureGeneratorStore featureGeneratorStore;

        public ScheduledFeatureGenerators(FeatureGeneratorWorkflows workflows, IFeatureGeneratorStore featureGeneratorStore)
        {
            this.workflows = workflows;
            this.featureGeneratorStore = featureGeneratorStore;
        }

        [Function("Run_QueuedFeatureGeneratorsForTenant")]
        [QueueOutput(AzureQueueNames.RunFeatureGenerator)]
        // This grabs all the generators and adds them each to another queue (fan out)
        public async Task<IEnumerable<RunFeatureGeneratorQueueMessage>> RunAllGeneratorsQueue(
            [QueueTrigger(AzureQueueNames.RunAllFeatureGenerators)] RunAllFeatureGeneratorsQueueMessage message, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(RunAllGeneratorsQueue));

            logger.LogInformation("Starting to run all feature generators.");

            var generators = await featureGeneratorStore.Query(1);
            logger.LogInformation($"Discovered {generators.Pagination.TotalItemCount} generators");
            if (generators.Pagination.HasNextPage)
            {
                logger.LogWarning("Generators are being skipped due to pagination!");
            }
            return generators.Items.Select(_ => new RunFeatureGeneratorQueueMessage(message.TenantName, _.Id));
        }

        [Function("Run_QueuedSingleFeatureGeneratorForTenant")]
        // this takes a generator and a tenant from a queue and runs the generator
        public async Task RunOneGeneratorFromQueue([QueueTrigger(AzureQueueNames.RunFeatureGenerator)] RunFeatureGeneratorQueueMessage message, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(RunOneGeneratorFromQueue));

            logger.LogInformation($"Starting to run one feature generator with id = {message.FeatureGeneratorId}.");
            var generator = await featureGeneratorStore.Read(message.FeatureGeneratorId);
            var summary = await workflows.RunFeatureGeneration(generator, subsetOnly: false);

            logger.LogInformation($"Finished RunOneGeneratorFromQueue for generator {generator.Id} with total writes: {summary.TotalWrites}");
        }



        // [Function("ManualStartFeatureGenerators")]
        // public async Task<HttpResponseData> ManualStart([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        //    FunctionContext executionContext)
        // {
        //     var logger = executionContext.GetLogger("ManualStartFeatureGenerators");

        //     logger.LogInformation("Starting to run all feature generators.");
        //     try
        //     {
        //         var totalWrites = await workflows.RunAllFeatureGenerators();
        //         var response = req.CreateResponse(HttpStatusCode.OK);
        //         await response.WriteAsJsonAsync(new { result = "Ok", totalWrites = totalWrites });

        //         return response;
        //     }
        //     catch (System.Exception ex)
        //     {
        //         logger.LogError("Error when running all workflows", ex);
        //         logger.LogError(ex.Message);
        //         var response = req.CreateResponse(HttpStatusCode.InternalServerError);
        //         await response.WriteAsJsonAsync(new { result = "Error", error = ex.Message, stackTrace = ex.StackTrace });
        //         response.StatusCode = HttpStatusCode.InternalServerError;
        //         return response;
        //     }

        // }
    }
}
