using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Constants;
using SignalBox.Core.Workflows;

namespace SignalBox.Functions
{
    public class MetricGeneratorJobs
    {
        private readonly MetricGeneratorWorkflows customerMetricWorkflows;
        private readonly BusinessMetricGeneratorWorkflow businessMetricWorkflow;
        private readonly AggregateCustomerMetricWorkflow aggregateCustomerMetricWorkflow;
        private readonly JoinTwoMetricsWorkflow joinTwoMetricsWorkflow;
        private readonly IEnvironmentProvider environmentProvider;
        private readonly IMetricGeneratorStore metricGeneratorStore;
        private readonly IDateTimeProvider dateTimeProvider;

        public MetricGeneratorJobs(MetricGeneratorWorkflows customerMetricWorkflows,
                                   AggregateCustomerMetricWorkflow aggregateCustomerMetricWorkflow,
                                   JoinTwoMetricsWorkflow joinTwoMetricsWorkflow,
                                   BusinessMetricGeneratorWorkflow businessMetricWorkflow,
                                   IEnvironmentProvider environmentProvider,
                                   IMetricGeneratorStore metricGeneratorStore,
                                   IDateTimeProvider dateTimeProvider)
        {
            this.customerMetricWorkflows = customerMetricWorkflows;
            this.aggregateCustomerMetricWorkflow = aggregateCustomerMetricWorkflow;
            this.joinTwoMetricsWorkflow = joinTwoMetricsWorkflow;
            this.businessMetricWorkflow = businessMetricWorkflow;
            this.environmentProvider = environmentProvider;
            this.metricGeneratorStore = metricGeneratorStore;
            this.dateTimeProvider = dateTimeProvider;
        }

        [Function("Run_QueuedMetricGeneratorsForTenant")]
        [QueueOutput(AzureQueueNames.RunMetricGenerator)]
        // This grabs all the generators and adds them each to another queue (fan out)
        public async Task<IEnumerable<RunMetricGeneratorQueueMessage>> RunAllGeneratorsQueue(
            [QueueTrigger(AzureQueueNames.RunAllMetricGenerators)] RunAllMetricGeneratorsQueueMessage message, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(RunAllGeneratorsQueue));

            logger.LogInformation("Starting to run all metric generators.");

            var generators = await metricGeneratorStore.Query(1);
            logger.LogInformation($"Discovered {generators.Pagination.TotalItemCount} generators");
            if (generators.Pagination.HasNextPage)
            {
                logger.LogWarning("Generators are being skipped due to pagination!");
            }
            // update the last enqueued time for all generators.
            foreach (var g in generators.Items)
            {
                g.LastEnqueued = dateTimeProvider.Now;
                await metricGeneratorStore.Update(g);
                await metricGeneratorStore.Load(g, _ => _.Metric);
            }
            await metricGeneratorStore.Context.SaveChanges();
            return generators.Items.Select(_ => new RunMetricGeneratorQueueMessage(message.TenantName, _.Id, _.Metric.EnvironmentId));
        }

        [Function("Run_QueuedSingleMetricGeneratorForTenant")]
        // this takes a generator and a tenant from a queue and runs the generator
        public async Task RunOneGeneratorFromQueue([QueueTrigger(AzureQueueNames.RunMetricGenerator)] RunMetricGeneratorQueueMessage message, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(RunOneGeneratorFromQueue));

            logger.LogInformation($"Starting to run one metric generator with id = {message.MetricGeneratorId}.");
            environmentProvider.SetOverride(message.EnvironmentId);
            var generator = await metricGeneratorStore.Read(message.MetricGeneratorId);
            await metricGeneratorStore.Load(generator, _ => _.Metric);
            logger.LogInformation("Loaded generator {generatorId} for metric {metricId}", generator.Id, generator.MetricId);

            if (generator.Metric.Scope == Core.Metrics.MetricScopes.Customer)
            {
                var summary = await customerMetricWorkflows.RunMetricGeneration(generator);
                logger.LogInformation($"Finished RunOneGeneratorFromQueue for generator {generator.Id} with total writes: {summary.TotalWrites}");
            }
            else if (generator.Metric.Scope == Core.Metrics.MetricScopes.Business)
            {
                var summary = await businessMetricWorkflow.RunBusinessMetricGeneration(generator);
                logger.LogInformation($"Finished RunOneGeneratorFromQueue for generator {generator.Id} with total writes: {summary.TotalWrites}");
            }
            else if (generator.Metric.Scope == Core.Metrics.MetricScopes.Global)
            {
                // global generators
                switch (generator.GeneratorType)
                {
                    case MetricGeneratorTypes.AggregateCustomerMetric:
                        await aggregateCustomerMetricWorkflow.RunAggregateCustomerMetricWorkflow(generator);
                        break;
                    case MetricGeneratorTypes.JoinTwoMetrics:
                        await joinTwoMetricsWorkflow.RunJoinTwoMetricWorkflow(generator);
                        break;
                    default:
                        throw new WorkflowException($"Unknown generator type for global scope: {generator.GeneratorType}");
                }
            }
            else
            {
                throw new WorkflowException($"Metric {generator.MetricId} has unknown scope {generator.Metric.Scope}");
            }


            generator.LastCompleted = dateTimeProvider.Now;
            await metricGeneratorStore.Update(generator);
            await metricGeneratorStore.Context.SaveChanges();
        }



        // [Function("ManualStartMetricGenerators")]
        // public async Task<HttpResponseData> ManualStart([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        //    FunctionContext executionContext)
        // {
        //     var logger = executionContext.GetLogger("ManualStartMetricGenerators");

        //     logger.LogInformation("Starting to run all metric generators.");
        //     try
        //     {
        //         var totalWrites = await workflows.RunAllMetricGenerators();
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
