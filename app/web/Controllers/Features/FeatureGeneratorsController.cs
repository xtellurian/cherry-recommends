using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Features.Generators;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class FeatureGeneratorsController : EntityControllerBase<FeatureGenerator>
    {
        private readonly ILogger<FeatureGeneratorsController> logger;
        private readonly IFeatureStore featureStore;
        private readonly IRunFeatureGeneratorQueueStore generatorQueue;
        private readonly ITenantProvider tenantProvider;
        private readonly IDateTimeProvider dateTimeProvider;

        public FeatureGeneratorsController(ILogger<FeatureGeneratorsController> logger,
                                           IFeatureStore featureStore,
                                           IRunFeatureGeneratorQueueStore generatorQueue,
                                           ITenantProvider tenantProvider,
                                           IDateTimeProvider dateTimeProvider,
                                           IFeatureGeneratorStore store) : base(store)
        {
            this.logger = logger;
            this.featureStore = featureStore;
            this.generatorQueue = generatorQueue;
            this.tenantProvider = tenantProvider;
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <summary>Returned a paginated list of items for this resource.</summary>
        [HttpGet]
        public override async Task<Paginated<FeatureGenerator>> Query([FromQuery] PaginateRequest p)
        {
            // include the feature
            return await store.Query(p.Page, _ => _.Feature);
        }

        /// <summary>Creates a new generic Feature that can used on any tracked user.</summary>
        [HttpPost]
        public async Task<FeatureGenerator> CreateFeatureGenerator([FromBody] CreateFeatureGenerator dto)
        {
            dto.Validate();
            if (dto.Steps != null)
            {
                foreach (var s in dto.Steps)
                {
                    s.Validate();
                }
            }

            if (System.Enum.TryParse<FeatureGeneratorTypes>(dto.GeneratorType, out var generatorType))
            {
                var feature = await featureStore.ReadFromCommonId(dto.FeatureCommonId);
                var generator = await store.Create(new FeatureGenerator(feature, generatorType, dto.Steps.ToCoreRepresentation()));
                await featureStore.Context.SaveChanges();
                return generator;
            }
            else
            {
                throw new BadRequestException($"{dto.GeneratorType} is an unknown generator type");
            }
        }

        [HttpPost("{id}/Trigger")]
        public async Task<FeatureGeneratorRunSummary> TriggerFeatureGenerator(long id)
        {
            var generator = await store.Read(id);

            if (generator.LastEnqueued < dateTimeProvider.Now.AddMinutes(-5))
            {
                throw new BadRequestException("Wait at least 5 minutes.", "Can't trigger a generator that was enqueued less than 5 minutes ago.");
            }

            var tenant = tenantProvider.Current();
            generator.LastEnqueued = dateTimeProvider.Now;

            await generatorQueue.Enqueue(new RunFeatureGeneratorQueueMessage(tenant?.Name ?? "_single", generator.Id));
            await store.Update(generator);
            await store.Context.SaveChanges();


            return new FeatureGeneratorRunSummary
            {
                Enqueued = true
            };
        }
    }
}
