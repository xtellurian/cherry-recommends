using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class BusinessesController : CommonEntityControllerBase<Business>
    {
        private readonly ILogger<BusinessesController> _logger;

        private readonly BusinessWorkflows workflows;

        public BusinessesController(ILogger<BusinessesController> logger,
                                      IBusinessStore store,
                                      BusinessWorkflows workflows) : base(store)
        {
            _logger = logger;
            this.workflows = workflows;
        }

        protected override Task<(bool, string)> CanDelete(Business entity)
        {
            return Task.FromResult((true, ""));
        }

        /// <summary>Adds a new Business.</summary>
        [HttpPost]
        public async Task<Business> CreateBusiness([FromBody] CreateBusinessDto dto)
        {
            return await workflows.CreateBusiness(dto.CommonId, dto.Name, dto.Description);
        }
    }
}
