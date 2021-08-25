using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Workflows;

namespace dotnetFunctions
{
    public class ManualStartFeatureGenerators
    {
        private readonly FeatureGeneratorWorkflows workflows;

        public ManualStartFeatureGenerators(FeatureGeneratorWorkflows workflows)
        {
            this.workflows = workflows;
        }

        [Function("ManualStartFeatureGenerators")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(ManualStartFeatureGenerators));

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
                await response.WriteAsJsonAsync(new { result = "Bad", error = ex.Message });

                return response;
            }

        }
    }
}
