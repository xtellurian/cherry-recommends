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
        private readonly IItemsRecommenderStore itemsRecommenderStore;

        // add all project dependencies to this ctor
        public TestBoot(
              CustomerEventsWorkflows eventWorkflow,
                        CustomerWorkflows customerWorkflows,
                        CustomerEventsWorkflows customerEventsWorkflows,
                        HubspotPushWorkflows hubspotPushWorkflows,
                        IItemsRecommenderStore itemsRecommenderStore,
                        IQueueMessagesFileStore queueMessagesFileStore,
                        ITenantStore tenantStore,
                        ITenantMembershipStore membershipStore,
                        IDatabaseManager databaseManager,
                        IAuth0Service auth0
                        )
        {
            this.itemsRecommenderStore = itemsRecommenderStore;
        }

        // public TestBoot() { }
        [Function("TestBoot")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("TestBoot");
            logger.LogInformation("C# test boot triggered.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            // check can connect to SQL Server by running a query
            var t = itemsRecommenderStore.Query();
            t.Wait();

            response.WriteString("Dependencies are OK");
            logger.LogInformation("Finished test boot process. Returning.");
            return response;
        }
    }
}
