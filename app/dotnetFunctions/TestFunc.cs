using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Workflows;

namespace SignalBox.Functions
{
    public class TestFunc
    {
        private readonly TrackedUserEventsWorkflows workflow;

        public TestFunc(TrackedUserEventsWorkflows workflow)
        {
            this.workflow = workflow;

            // test the DI
        }
        [Function("TestFunc")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("TestFunc");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
