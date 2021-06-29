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
    [Route("api/models/[controller]")]
    public class AzureSingleClassClassifierController : SignalBoxControllerBase
    {
        private readonly InvokeModelWorkflows workflows;
        private readonly IModelRegistrationStore modelRegistrationStore;

        public AzureSingleClassClassifierController(InvokeModelWorkflows workflows,
                                                    IModelRegistrationStore modelRegistrationStore)
        {
            this.workflows = workflows;
            this.modelRegistrationStore = modelRegistrationStore;
        }

        /// <summary>Invoke a model with some payload.</summary>
        [HttpPost("{id}/invoke")]
        public async Task<AzureMLClassifierOutput> InvokeModel(long id, string version, [FromBody] AzureMLModelInput input)
        {
            return await workflows.InvokeClassifierModel(id, version, input);
        }
    }
}