using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Security;
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

        public FeatureGeneratorsController(ILogger<FeatureGeneratorsController> logger,
                                           IFeatureStore featureStore,
                                           IFeatureGeneratorStore store) : base(store)
        {
            this.logger = logger;
            this.featureStore = featureStore;
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
        [Authorize(Policies.AdminOnlyPolicyName)]
        public async Task<FeatureGenerator> CreateFeatureGenerator([FromBody] CreateFeatureGenerator dto)
        {
            if (System.Enum.TryParse<FeatureGeneratorTypes>(dto.GeneratorType, out var generatorType))
            {
                var feature = await featureStore.ReadFromCommonId(dto.FeatureCommonId);
                var generator = await store.Create(new FeatureGenerator(feature, generatorType));
                await featureStore.Context.SaveChanges();
                return generator;
            }
            else
            {
                throw new BadRequestException($"{dto.GeneratorType} is an unknown generator type");
            }
        }
    }
}
