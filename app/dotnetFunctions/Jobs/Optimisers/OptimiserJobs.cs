using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using SignalBox.Core;
using System.Collections.Generic;
using System.Text;
using SignalBox.Core.Optimisers;

namespace SignalBox.Functions
{
    public class OptimiserJobs
    {
        readonly JsonSerializerOptions deSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };
        readonly JsonSerializerOptions serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };


        [Function("CreateCategoricalOptimiser")]
        public async Task<CreateCategoricalOptimiserResponse> TriggerCreateCategoricalOptimiser([HttpTrigger(AuthorizationLevel.Function, "post",
            Route = "v1/{tenant}/categorical")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("CreateCategoricalOptimiser");
            logger.LogInformation("Creating a categorical optimiser");

            CategoricalOptimiser info = null;
            try
            {
                info = await JsonSerializer.DeserializeAsync<CategoricalOptimiser>(req.Body, deSerializerOptions);
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Body of request must be JSON", ex);
            }

            var tenant = executionContext.BindingContext.BindingData["tenant"].ToString();
            PopulationDistributionCollection distributions = CreateCategoricalOptimiser(info, tenant);

            // # save it to a blob for use later.
            byte[] blob = JsonSerializer.SerializeToUtf8Bytes(distributions, serializerOptions);

            // # create a dict of the data you need to store
            CategoricalOptimiserRecord recordData = new CategoricalOptimiserRecord()
            {
                PartitionKey = tenant,
                RowKey = info.Id,
                Tenant = tenant,
                Id = info.Id,
                Name = info.Name,
                NItemsToRecommend = info.NItemsToRecommend,
                NPopulations = 1,
                DefaultItem = info.DefaultItem,
                DefaultItemCommonId = info.DefaultItem.CommonId
            };

            // return the row data to the HTTP caller.
            var httpResponse = req.CreateResponse(HttpStatusCode.OK);
            await httpResponse.WriteAsJsonAsync(recordData);

            // create the multiple output bindings response
            return new CreateCategoricalOptimiserResponse()
            {
                HttpResponse = httpResponse,
                OutputBlob = blob,
                Record = recordData
            };
        }

        public PopulationDistributionCollection CreateCategoricalOptimiser(CategoricalOptimiser info, string tenant)
        {
            try
            {
                CategoricalOptimiserValidator.Validate(info);
            }
            catch (Exception)
            {
                throw;
            }

            // create some intitial distribution used in model
            PopulationDistributionCollection distributions = new PopulationDistributionCollection()
            {
                DefaultItem = info.DefaultItem,
                NItemsToRecommend = info.NItemsToRecommend
            };

            var populationDistribution = new PopulationItemDistribution(info.DefaultItem, info.Items);
            populationDistribution.NormalizeProbabilities();
            distributions.Add(populationDistribution);

            return distributions;
        }

        [Function("InvokeCategoricalOptimiser")]

        public async Task<HttpResponseData> TriggerInvokeCategoricalOptimiser([HttpTrigger(AuthorizationLevel.Function, "post",
            Route = "v1/{tenant}/categorical/{id}/invoke")] HttpRequestData req,
            // [TableInput("CategoricalOptimisers", "{tenant}", "{id}")] CategoricalOptimiserRecord record,
            [BlobInput("{tenant}/categorical-optimisers/{id}.json")] string inputBlob,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("InvokeCategoricalOptimiser");
            logger.LogInformation("Invoking a categorical optimiser");

            PopulationDistributionCollection collection = null;
            InvokeCategoricalOptimiserModel payloadInfo = null;

            try
            {
                collection = JsonSerializer.Deserialize<PopulationDistributionCollection>(inputBlob, deSerializerOptions);
                payloadInfo = await JsonSerializer.DeserializeAsync<InvokeCategoricalOptimiserModel>(req.Body, deSerializerOptions);
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Body of request must be JSON", ex);
            }

            List<ScoredItem> chosenItems = InvokeCategoricalOptimiser(collection, payloadInfo);

            var response = req.CreateResponse(HttpStatusCode.OK);
            InvokeCategoricalOptimiserResponse invokeResponse = new InvokeCategoricalOptimiserResponse()
            {
                ScoredItems = chosenItems
            };

            await response.WriteAsJsonAsync(invokeResponse);
            return response;
        }

        public List<ScoredItem> InvokeCategoricalOptimiser(PopulationDistributionCollection collection, InvokeCategoricalOptimiserModel payloadInfo)
        {
            try
            {
                payloadInfo.Validate();
            }
            catch (Exception)
            {
                throw;
            }

            var items = payloadInfo.Payload.Items;

            // # Get the population id given specific personalisation Metrics
            var populationId = collection.CalculatePopulationId(payloadInfo);

            var relevantPopulation = collection.GetPopulation(populationId);
            if (relevantPopulation == null)
            {
                relevantPopulation = new PopulationItemDistribution(collection.DefaultItem, items, populationId);
            }

            // # Draw the items to recommend with their scores
            List<ScoredItem> chosenItems = relevantPopulation.ChooseItems(items, collection.NItemsToRecommend);
            return chosenItems;
        }
    }
}
