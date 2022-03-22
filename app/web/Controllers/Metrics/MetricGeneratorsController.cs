using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Metrics.Generators;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    [Route("api/FeatureGenerators")]
    public class MetricGeneratorsController : EntityControllerBase<MetricGenerator>
    {
        private readonly ILogger<MetricGeneratorsController> logger;
        private readonly IMetricStore metricStore;
        private readonly IRunMetricGeneratorQueueStore generatorQueue;
        private readonly ITenantProvider tenantProvider;
        private readonly IDateTimeProvider dateTimeProvider;

        public MetricGeneratorsController(ILogger<MetricGeneratorsController> logger,
                                           IMetricStore metricStore,
                                           IRunMetricGeneratorQueueStore generatorQueue,
                                           ITenantProvider tenantProvider,
                                           IDateTimeProvider dateTimeProvider,
                                           IMetricGeneratorStore store) : base(store)
        {
            this.logger = logger;
            this.metricStore = metricStore;
            this.generatorQueue = generatorQueue;
            this.tenantProvider = tenantProvider;
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <summary>Returned a paginated list of items for this resource.</summary>
        [HttpGet]
        public override async Task<Paginated<MetricGenerator>> Query([FromQuery] PaginateRequest p)
        {
            // include the feature
            return await store.Query(_ => _.Metric, new EntityStoreQueryOptions<MetricGenerator>(p));
        }

        /// <summary>
        /// Creates a new Metric generator
        /// </summary>
        [HttpPost]
        public async Task<MetricGenerator> CreateMetricGenerator([FromBody] CreateMetricGenerator dto)
        {
            dto.Validate();
            if (dto.Steps != null)
            {
                foreach (var s in dto.Steps)
                {
                    s.Validate();
                }
            }

            var metric = await metricStore.ReadFromCommonId(dto.FeatureCommonId);
            MetricGenerator generator;
            switch (dto.GeneratorType)
            {
                case MetricGeneratorTypes.FilterSelectAggregate:
                    generator = MetricGenerator.CreateFilterSelectAggregateGenerator(metric, dto.Steps.ToCoreRepresentation(), dto.TimeWindow);
                    break;
                case MetricGeneratorTypes.AggregateCustomerMetric:
                    generator = MetricGenerator.CreateAggregateCustomerMetric(metric, dto.AggregateCustomerMetric.ToCoreRepresentation());
                    break;
                case MetricGeneratorTypes.JoinTwoMetrics:
                    generator = MetricGenerator.CreateJoinTwoGlobalMetric(metric, dto.JoinTwoMetrics.ToCoreRepresentation());
                    break;
                default:
                    throw new BadRequestException($"{dto.GeneratorType} is an unknown generator type");
            }

            generator = await store.Create(generator);
            await store.Context.SaveChanges();
            return generator;
        }

        [HttpPost("{id}/Trigger")]
        public async Task<MetricGeneratorRunSummary> TriggerMetricGenerator(long id)
        {
            var generator = await store.Read(id);
            await store.Load(generator, _ => _.Metric);

            if (generator.LastEnqueued > dateTimeProvider.Now.AddMinutes(-5))
            {
                throw new BadRequestException("Wait at least 5 minutes.", "Can't trigger a generator that was enqueued less than 5 minutes ago.");
            }

            var tenant = tenantProvider.Current();
            generator.LastEnqueued = dateTimeProvider.Now;

            await generatorQueue.Enqueue(new RunMetricGeneratorQueueMessage(tenant?.Name ?? "_single", generator.Id, generator.Metric.EnvironmentId));
            await store.Update(generator);
            await store.Context.SaveChanges();


            return new MetricGeneratorRunSummary
            {
                Enqueued = true
            };
        }
    }
}
