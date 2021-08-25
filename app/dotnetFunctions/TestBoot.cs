using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Workflows;

namespace dotnetFunctions
{
    public class TestBoot
    {
        // add all project dependencies to this ctor
        public TestBoot(TrackedUserEventsWorkflows eventWorkflow,
                        TrackedUserWorkflows trackedUserWorkflows,
                        IQueueMessagesFileStore queueMessagesFileStore)
        {

        }
        [Function("TestBoot")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("TestBoot");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Dependencies are OK");

            return response;
        }
    }
}
