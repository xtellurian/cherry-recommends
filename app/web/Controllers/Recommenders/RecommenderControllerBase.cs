
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalBox.Core;
using SignalBox.Core.Recommenders;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    public class RecommenderControllerBase<T> : CommonEntityControllerBase<T> where T : RecommenderEntityBase
    {

        public RecommenderControllerBase(ICommonEntityStore<T> store) : base(store)
        { }

        [HttpGet("{id}/TargetVariableValues")]
        public async Task<IEnumerable<RecommenderTargetVariableValue>> TargetVariableValues(string id, string name = null)
        {
            var recommender = await base.GetEntity(id, null, _ => _.TargetVariableValues);
            if (string.IsNullOrEmpty(name) || name == "null")
            {
                return recommender.TargetVariableValues.ToList().OrderBy(_ => _.Version);
            }
            else
            {
                return recommender.TargetVariableValues.Where(_ => _.Name == name).ToList().OrderBy(_ => _.Version);
            }
        }

        [HttpPost("{id}/TargetVariableValues")]
        [Authorize(Core.Security.Policies.AdminOnlyPolicyName)]
        public async Task<RecommenderTargetVariableValue> SetLatestTargetVariableValue(string id, CreateTargetVariableValue dto)
        {
            var recommender = await base.GetEntity(id, null, _ => _.TargetVariableValues);
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
