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
    [Route("api/models/generic")]
    public class GenericModelInvokerController : SignalBoxControllerBase
    {
        private readonly GenericModelWorkflows workflows;
        private readonly IModelRegistrationStore modelRegistrationStore;

        public GenericModelInvokerController(GenericModelWorkflows workflows, IModelRegistrationStore modelRegistrationStore)
        {
            this.workflows = workflows;
            this.modelRegistrationStore = modelRegistrationStore;
        }

        /// <summary>Invoke a model with some payload.</summary>
        [HttpPost("{id}/invoke")]
        public async Task<string> InvokeModel(long id)
        {
            var raw = await Request.GetRawBodyStringAsync();
            var (content, success) = await workflows.InvokeGeneric(id, raw);
            if (success)
            {
                return content;
            }
            else
            {
                throw new ModelInvokationException(content);
            }
        }
    }
}