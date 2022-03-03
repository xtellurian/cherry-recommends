using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ICustomerStore customerStore;

        public BusinessesController(ILogger<BusinessesController> logger,
                                    IBusinessStore store,
                                    ICustomerStore customerStore,
                                    BusinessWorkflows workflows) : base(store)
        {
            _logger = logger;
            this.workflows = workflows;
            this.customerStore = customerStore;
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

        /// <summary>Gets the Customers that are members of a Business.</summary>
        [HttpGet("{id}/Members")]
        public async Task<Paginated<Customer>> GetBusinessMembers(string id, [FromQuery] PaginateRequest p, [FromQuery] SearchEntities q)
        {
            return await customerStore.Query(new EntityStoreQueryOptions<Customer>(p.Page, _ => _.BusinessMembership.BusinessId == Int64.Parse(id) && 
                (EF.Functions.Like(_.CommonId, $"%{q.Term}%") || EF.Functions.Like(_.Name, $"%{q.Term}%"))));
        }
        
        [HttpDelete("{id}/Members/{customerId}")]
        public async Task<Customer> RemoveBusinessMembership(string id, long customerId)
        {
            var business = await base.GetResource(id);
            var customer = await workflows.RemoveBusinessMembership(business, customerId);
            return customer;
        }
    }
}
