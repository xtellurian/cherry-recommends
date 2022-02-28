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

        public BusinessesController(ILogger<BusinessesController> logger,
                                      IBusinessStore store) : base(store)
        {
            _logger = logger;
        }

        protected override Task<(bool, string)> CanDelete(Business entity)
        {
            return Task.FromResult((true, ""));
        }
    }
}
