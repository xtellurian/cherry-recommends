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
    [Route("api/[controller]")]
    public class ModelsController : SignalBoxControllerBase
    {
        private readonly IStorageContext storageContext;
        private readonly ModelRegistrationWorkflows workflows;
        private readonly IModelRegistrationStore modelRegistrationStore;

        public ModelsController(IStorageContext storageContext, ModelRegistrationWorkflows workflows, IModelRegistrationStore modelRegistrationStore)
        {
            this.storageContext = storageContext;
            this.workflows = workflows;
            this.modelRegistrationStore = modelRegistrationStore;
        }

        /// <summary>Invoke a model with some payload.</summary>
        [HttpPost("{id}/invoke")]
        public async Task<EvaluationResult> InvokeModel(long id, [FromBody] AzureMLModelPayload azML)
        {
            return await workflows.InvokeModel(id, azML.Data.FirstOrDefault());
        }
    }
}