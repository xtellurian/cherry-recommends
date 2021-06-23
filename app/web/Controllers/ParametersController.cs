using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto.Parameters;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class ParametersController : CommonEntityControllerBase<Parameter>
    {
        private readonly ParameterWorkflows workflows;

        public ParametersController(IParameterStore store, ParameterWorkflows workflows) : base(store)
        {
            this.workflows = workflows;
        }

        [HttpPost]
        public async Task<Parameter> CreateParameter(CreateParameter dto)
        {
            var common = new CreateCommonEntityModel(dto.CommonId, dto.Name);
            return await workflows.CreateParameter(new CreateParameterModel(common, dto.ParameterType, dto.Description));
        }
    }
}