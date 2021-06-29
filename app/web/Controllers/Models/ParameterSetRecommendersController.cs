using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalBox.Core;
using SignalBox.Core.Workflows;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/models/ParameterSetRecommenders")]
    public class ParameterSetRecommenderModelsController : SignalBoxControllerBase
    {
        private readonly InvokeModelWorkflows workflows;
        private readonly IModelRegistrationStore modelRegistrationStore;

        public ParameterSetRecommenderModelsController(InvokeModelWorkflows workflows,
                                                       IModelRegistrationStore modelRegistrationStore)
        {
            this.workflows = workflows;
            this.modelRegistrationStore = modelRegistrationStore;
        }

        /// <summary>Invoke a model with some payload. Id is the model Id.</summary>
        [HttpPost("{id}/invoke")]
        public async Task<ParameterSetRecommenderModelOutputV1> InvokeModel(long id, string version, [FromBody] ParameterSetRecommenderModelInputV1 input)
        {
            return await workflows.InvokeParameterSetRecommender(id, version, input);
        }
    }
}