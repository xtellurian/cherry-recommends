
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;
using SignalBox.Web.Dto.RecommenderInputs;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/recommenders/[controller]")]
    public class ProductRecommendersController : RecommenderControllerBase<ProductRecommender>
    {
        private readonly ILogger<ProductRecommendersController> logger;
        private readonly ProductRecommenderInvokationWorkflows invokationWorkflows;
        private readonly ProductRecommenderWorkflows workflows;

        public ProductRecommendersController(ILogger<ProductRecommendersController> logger,
                                                 IProductRecommenderStore store,
                                                 ProductRecommenderInvokationWorkflows invokationWorkflows,
                                                 ProductRecommenderWorkflows workflows) : base(store, invokationWorkflows)
        {
            this.logger = logger;
            this.invokationWorkflows = invokationWorkflows;
            this.workflows = workflows;
        }

        /// <summary>Returns the resource with this Id.</summary>
        [HttpGet("{id}")]
        public override async Task<ProductRecommender> GetResource(string id, bool? useInternalId = null)
        {
            var recommender = await base.GetEntity(id, useInternalId, _ => _.DefaultProduct); // include the default product
            await store.LoadMany(recommender, _ => _.Products);
            return recommender;
        }

        /// <summary>Creates a new product recommender.</summary>
        [HttpPost]
        public async Task<ProductRecommender> Create(CreateProductRecommender dto, bool? useInternalId = null)
        {
            return await workflows.CreateProductRecommender(
                new CreateCommonEntityModel(dto.CommonId, dto.Name),
                dto.Touchpoint, dto.DefaultProductId, dto.ProductIds,
                new RecommenderErrorHandling { ThrowOnBadInput = dto.ThrowOnBadInput });
        }

        /// <summary>Sets the default product id.</summary>
        [HttpPost("{id}/DefaultProduct")]
        public async Task<Product> SetDefaultProduct(string id, [FromBody] DefaultProductDto dto, bool? useInternalId = null)
        {
            var recommender = await GetEntity(id, useInternalId);
            return await workflows.SetDefaultProduct(recommender, dto.ProductId);
        }

        /// <summary>Sets the default product id.</summary>
        [HttpGet("{id}/DefaultProduct")]
        public async Task<Product> GetDefaultProduct(string id, bool? useInternalId = null)
        {
            var recommender = await GetEntity(id, useInternalId);
            await store.Load(recommender, _ => _.DefaultProduct);
            return recommender.DefaultProduct ?? throw new BadRequestException("Recommender has no default product");
        }

        /// <summary>Set the backing model information.</summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("{id}/ModelRegistration")]
        public async Task<ModelRegistration> LinkModel(string id, LinkModel dto)
        {
            var recommender = await base.GetResource(id);
            return await workflows.LinkRegisteredModel(recommender, dto.ModelId);
        }

        /// <summary>Get the backing model information.</summary>
        [HttpGet("{id}/ModelRegistration")]
        public async Task<ModelRegistration> GetLinkModel(string id)
        {
            var parameterSetRecommender = await base.GetResource(id);
            return parameterSetRecommender.ModelRegistration ??
                throw new EntityNotFoundException(typeof(ModelRegistration), id);

        }

        /// <summary>Invoke a model with some input. Id is the recommender Id.</summary>
        [HttpPost("{id}/invoke")]
        public async Task<ProductRecommenderModelOutputV1> InvokeModel(
            string id,
            string version,
            [FromBody] ProductRecommenderInput input,
            bool? useInternalId = null)
        {
            var recommender = await base.GetResource(id, useInternalId);
            var convertedInput = new ProductRecommenderModelInputV1
            {
                Arguments = input.Arguments,
                CommonUserId = input.CommonUserId,
            };
            return await invokationWorkflows.InvokeProductRecommender(recommender, version, convertedInput);

        }

        /// <summary>Get the latest recommendations made by a recommender.</summary>
        [HttpGet("{id}/recommendations")]
        public async Task<Paginated<ProductRecommendation>> GetRecommendations(long id, [FromQuery] PaginateRequest p)
        {
            return await workflows.QueryRecommendations(id, p.Page);
        }

        protected override Task<(bool, string)> CanDelete(ProductRecommender entity)
        {
            return Task.FromResult((true, ""));
        }
    }
}