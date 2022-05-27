using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalBox.Core;
using SignalBox.Core.Optimisers;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/Recommenders/PromotionsRecommenders/{id}/Optimiser")]
    [Route("api/Campaigns/PromotionsCampaigns/{id}/Optimiser")]
    public class PromotionOptimiserController : SignalBoxControllerBase
    {
        private readonly IPromotionOptimiserCRUDWorkflow optimiserCRUDWorkflow;

        public PromotionOptimiserController(IPromotionOptimiserCRUDWorkflow optimiserCRUDWorkflow)
        {
            this.optimiserCRUDWorkflow = optimiserCRUDWorkflow;
        }

        [HttpGet]
        public async Task<PromotionOptimiser> Get(string id, bool? useInternalId = null)
        {
            return await optimiserCRUDWorkflow.Read(id, useInternalId);
        }

        [HttpPost("Weights")]
        public async Task<PromotionOptimiser> SetAllWeights(string id, [FromBody] IEnumerable<UpdateWeightDto> dto, bool? useInternalId = null)
        {
            if (dto.IsNullOrEmpty())
            {
                throw new BadRequestException("Weights must not be null or empty");
            }
            return await optimiserCRUDWorkflow.UpdateAllWeights(id, dto, useInternalId);
        }

        [HttpPost("Weights/{weightId}")]
        public async Task<PromotionOptimiser> SetWeight(string id, long weightId, [FromBody] UpdateWeightDto dto, bool? useInternalId = null)
        {
            return await optimiserCRUDWorkflow.UpdateWeight(id, weightId, dto.Weight, useInternalId);
        }
    }
}