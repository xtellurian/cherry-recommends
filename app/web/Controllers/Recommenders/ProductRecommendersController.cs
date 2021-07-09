
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Recommendations;
using SignalBox.Core.Recommenders;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/recommenders/[controller]")]
    public class ProductRecommendersController : CommonEntityControllerBase<ProductRecommender>
    {
        private readonly ILogger<ProductRecommendersController> logger;
        private readonly ProductRecommenderInvokationWorkflows modelWorkflows;
        private readonly ProductRecommenderWorkflows workflows;

        public ProductRecommendersController(ILogger<ProductRecommendersController> logger,
                                                 IProductRecommenderStore store,
                                                 ProductRecommenderInvokationWorkflows modelWorkflows,
                                                 ProductRecommenderWorkflows workflows) : base(store)
        {
            this.logger = logger;
            this.modelWorkflows = modelWorkflows;
            this.workflows = workflows;
        }

        /// <summary>Creates a new product recommender.</summary>
        [HttpPost]
        public async Task<ProductRecommender> Create(CreateProductRecommender dto)
        {
            return await workflows.CreateProductRecommender(new CreateCommonEntityModel(dto.CommonId, dto.Name), dto.Touchpoint, dto.ProductIds);
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
                throw new EntityNotFoundException(typeof(ModelRegistration), id, null);

        }

        /// <summary>Invoke a model with some input. Id is the recommender Id.</summary>
        [HttpPost("{id}/invoke")]
        public async Task<ProductRecommenderModelOutputV1> InvokeModel(long id, string version, [FromBody] ProductRecommenderModelInputV1 input)
        {
            return await modelWorkflows.InvokeProductRecommender(id, version, input);
        }

        /// <summary>Get the latest recommendations made by a recommender.</summary>
        [HttpGet("{id}/recommendations")]
        public async Task<Paginated<ProductRecommendation>> GetRecommendations(long id, [FromQuery] PaginateRequest p)
        {
            return await modelWorkflows.QueryRecommendations(id, p.Page);
        }

    }
}