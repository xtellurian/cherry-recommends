using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalBox.Core;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class EnvironmentsController : EntityControllerBase<Environment>
    {
        public EnvironmentsController(IEnvironmentStore store) : base(store)
        { }

        [HttpPost]
        public async Task<Environment> CreateEnvironment(CreateEnvironment dto)
        {
            if (await store.Count() > 3)
            {
                throw new BadRequestException("The maximum number of environments is 3");
            }

            var environment = await store.Create(new Environment(dto.Name));
            await store.Context.SaveChanges();
            return environment;
        }
    }
}