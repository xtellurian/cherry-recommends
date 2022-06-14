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

        [HttpGet("Weights")]
        public async Task<IEnumerable<PromotionOptimiserWeight>> GetDefaultWeights(string id, bool? useInternalId = null)
        {
            return await optimiserCRUDWorkflow.ReadWeights(id, null, useInternalId);
        }

        [HttpPost("Weights")]
        public async Task<PromotionOptimiser> SetAllWeights(string id, [FromBody] IEnumerable<UpdateWeightDto> dto, bool? useInternalId = null)
        {
            if (dto.IsNullOrEmpty())
            {
                throw new BadRequestException("Weights must not be null or empty");
            }
            return await optimiserCRUDWorkflow.UpdateAllWeights(id, dto, null, useInternalId);
        }

        [HttpPost("Weights/{weightId}")]
        public async Task<PromotionOptimiser> SetWeight(string id, long weightId, [FromBody] UpdateWeightDto dto, bool? useInternalId = null)
        {
            return await optimiserCRUDWorkflow.UpdateWeight(id, weightId, dto.Weight, null, useInternalId);
        }

        [HttpGet("Segments")]
        public async Task<Paginated<SignalBox.Core.Segment>> GetSegments(string id, bool? useInternalId = null)
        {
            return await optimiserCRUDWorkflow.ReadSegments(id, useInternalId);
        }

        [HttpPost("Segments")]
        public async Task<PromotionOptimiser> AddSegment(string id, [FromBody] AddOptimiserSegmentDto dto, bool? useInternalId = null)
        {
            return await optimiserCRUDWorkflow.AddSegment(id, dto.SegmentId);
        }

        [HttpDelete("Segments/{segmentId}")]
        public async Task<DeleteResponse> RemoveSegment(string id, long segmentId, bool? useInternalId = null)
        {
            bool success = await optimiserCRUDWorkflow.RemoveSegment(id, segmentId, useInternalId);
            return new DeleteResponse(segmentId, Request.Path.Value, success);
        }

        [HttpGet("Segments/{segmentId}/Weights")]
        public async Task<IEnumerable<PromotionOptimiserWeight>> GetSegmentWeights(string id, long segmentId, bool? useInternalId = null)
        {
            return await optimiserCRUDWorkflow.ReadWeights(id, segmentId, useInternalId);
        }

        [HttpPost("Segments/{segmentId}/Weights")]
        public async Task<PromotionOptimiser> SetSegmentWeights(string id, long segmentId, [FromBody] IEnumerable<UpdateWeightDto> dto, bool? useInternalId = null)
        {
            if (dto.IsNullOrEmpty())
            {
                throw new BadRequestException("Weights must not be null or empty");
            }
            return await optimiserCRUDWorkflow.UpdateAllWeights(id, dto, segmentId, useInternalId);
        }

        [HttpPost("Segments/{segmentId}/Weights/{weightId}")]
        public async Task<PromotionOptimiser> SetSegmentWeight(string id, long segmentId, long weightId, [FromBody] UpdateWeightDto dto, bool? useInternalId = null)
        {
            return await optimiserCRUDWorkflow.UpdateWeight(id, weightId, dto.Weight, segmentId, useInternalId);
        }
    }
}