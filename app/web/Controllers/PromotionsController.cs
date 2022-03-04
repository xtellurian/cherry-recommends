using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/RecommendableItems")]
    [Route("api/[controller]")]
    public class PromotionsController : CommonEntityControllerBase<RecommendableItem>
    {
        private readonly RecommendableItemWorkflows workflows;

        public PromotionsController(IRecommendableItemStore store, RecommendableItemWorkflows workflows) : base(store)
        {
            this.workflows = workflows;
        }

        /// <summary>Returned a paginated list of items for this resource.</summary>
        [HttpGet]
        public override async Task<Paginated<RecommendableItem>> Query([FromQuery] PaginateRequest p, [FromQuery] SearchEntities q)
        {
            string[] promotionTypes = HttpContext.Request.Query["promotionType"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string[] benefitTypes = HttpContext.Request.Query["benefitType"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            List<Expression<Func<RecommendableItem, bool>>> expressions = new List<Expression<Func<RecommendableItem, bool>>>();
            Expression<Func<RecommendableItem, bool>> predicate = null;

            if (promotionTypes.Length > 0)
            {
                var enums = promotionTypes.Select(_ => (PromotionType)Enum.Parse(typeof(PromotionType), _, ignoreCase: true));
                expressions.Add(_ => enums.Contains(_.PromotionType));
            }
            if (benefitTypes.Length > 0)
            {
                var enums = benefitTypes.Select(_ => (BenefitType)Enum.Parse(typeof(BenefitType), _, ignoreCase: true));
                expressions.Add(_ => enums.Contains(_.BenefitType));
            }
            if (!string.IsNullOrEmpty(q.Term))
            {
                expressions.Add(_ => EF.Functions.Like(_.CommonId, $"%{q.Term}%") || EF.Functions.Like(_.Name, $"%{q.Term}%"));
            }
            if (q.WeeksAgo.HasValue)
            {
                expressions.Add(_ => EF.Functions.DateDiffWeek(_.Created, DateTime.UtcNow) <= q.WeeksAgo.Value);
            }

            foreach (var expression in expressions)
            {
                predicate = predicate != null ? predicate.And(expression) : expression;
            }

            return await store.Query(p.Page, predicate);
        }

        /// <summary>Creates a new recommendable promotion.</summary>
        [HttpPost]
        public async Task<RecommendableItem> Create(CreatePromotionDto dto)
        {
            return await workflows.CreateRecommendableItem(dto.CommonId, dto.Name, dto.DirectCost, dto.NumberOfRedemptions, dto.BenefitType, dto.BenefitValue, dto.PromotionType, dto.Description, dto.Properties);
        }

        /// <summary>Updates a recommendable promotion.</summary>
        [HttpPost("{id}")]
        public async Task<RecommendableItem> Update(string id, UpdatePromotionDto dto)
        {
            var entity = await store.GetEntity(id);
            return await workflows.UpdateRecommendableItem(entity, dto.Name, dto.DirectCost, dto.NumberOfRedemptions, dto.BenefitType, dto.BenefitValue, dto.PromotionType, dto.Description, dto.Properties);
        }

        protected override async Task<(bool, string)> CanDelete(RecommendableItem entity)
        {
            var isBaselineItem = await workflows.IsBaselineItemForRecommender(entity);
            if (isBaselineItem)
            {
                return (false, "Cannot delete promotion as it is used as a Recommender's baseline promotion");
            }
            else
            {
                return (true, string.Empty);
            }
        }
    }
}