using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Infrastructure;

namespace dotnetFunctions
{
    public class TestBoot
    {
        // add all project dependencies to this ctor
        public TestBoot(
              TrackedUserEventsWorkflows eventWorkflow,
                        TrackedUserWorkflows trackedUserWorkflows,
                        HubspotPushWorkflows hubspotPushWorkflows,
                        IItemsRecommenderStore itemsRecommenderStore,
                        IQueueMessagesFileStore queueMessagesFileStore,
                        ITenantStore tenantStore,
                        ITenantMembershipStore membershipStore,
                        IDatabaseManager databaseManager,
                        IAuth0Service auth0
                        )
        { }

        // public TestBoot() { }
        [Function("TestBoot")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("TestBoot");
            logger.LogInformation("C# test boot triggered.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Dependencies are OK");
            logger.LogInformation("Finished test boot process. Returning.");
            return response;
        }
    }
}
