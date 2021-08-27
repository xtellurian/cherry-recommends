
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalBox.Core;
using SignalBox.Core.Recommenders;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    public abstract class RecommenderControllerBase<T> : CommonEntityControllerBase<T> where T : RecommenderEntityBase
    {
        private readonly RecommenderInvokationWorkflowBase<T> workflows;
        private readonly IRecommenderStore<T> recommenderStore;

        protected RecommenderControllerBase(IRecommenderStore<T> store, RecommenderInvokationWorkflowBase<T> workflows) : base(store)
        {
            this.workflows = workflows;
            this.recommenderStore = store;
        }

        [HttpGet("{id}/InvokationLogs")]
        public async Task<Paginated<InvokationLogEntry>> GetInvokationLogs(string id, [FromQuery] PaginateRequest p, bool? useInternalId = null)
        {
            var recommender = await base.GetEntity(id, useInternalId);
            return await workflows.QueryInvokationLogs(recommender, p.Page);
        }

        [HttpGet("{id}/TargetVariableValues")]
        public async Task<IEnumerable<RecommenderTargetVariableValue>> TargetVariableValues(string id, string name = null, bool? useInternalId = null)
        {
            var recommender = await base.GetEntity(id, useInternalId);
            await store.LoadMany(recommender, _ => _.TargetVariableValues);
            if (string.IsNullOrEmpty(name) || name == "null")
            {
                return recommender.TargetVariableValues.ToList().OrderBy(_ => _.Version);
            }
            else
            {
                return recommender.TargetVariableValues.Where(_ => _.Name == name).ToList().OrderBy(_ => _.Version);
            }
        }

        [HttpGet("{id}/TrackedUserActions")]
        public async Task<Paginated<TrackedUserAction>> GetAssociatedTrackedUserActions([FromQuery] PaginateRequest p,
                                                                                        string id,
                                                                                        bool? revenueOnly)
        {
            var recommender = await base.GetEntity(id, null);
            return await recommenderStore.QueryAssociatedActions(recommender, p.Page, revenueOnly ?? false);
        }

        [HttpPost("{id}/ErrorHandling")]
        public async Task<RecommenderErrorHandling> SetErrorHandling(string id, RecommenderErrorHandling dto)
        {
            var recommender = await base.GetResource(id);
            recommender.ErrorHandling = dto;
            await store.Update(recommender);
            await store.Context.SaveChanges();
            return recommender.ErrorHandling;
        }

        [HttpPost("{id}/TargetVariableValues")]
        [Authorize(Core.Security.Policies.AdminOnlyPolicyName)]
        public async Task<RecommenderTargetVariableValue> SetLatestTargetVariableValue(string id, CreateTargetVariableValue dto)
        {
            var recommender = await base.GetEntity(id, null);
            await store.LoadMany(recommender, _ => _.TargetVariableValues);
            var currentMaxVersion = recommender.TargetVariableValues
                .Where(_ => _.Name == dto.Name)
                .Max(_ => (int?)_.Version) ?? 0;

            var value = new RecommenderTargetVariableValue(recommender.Id, currentMaxVersion + 1, dto.Start, dto.End, dto.Name, dto.Value);
            recommender.TargetVariableValues.Add(value);
            await store.Context.SaveChanges();
            return value;
        }
    }

}
