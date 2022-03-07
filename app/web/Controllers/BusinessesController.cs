using System.Collections.Generic;
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
        private readonly ICustomerEventStore customerEventStore;

        public BusinessesController(ILogger<BusinessesController> logger,
                                    IBusinessStore store,
                                    ICustomerStore customerStore,
                                    ICustomerEventStore customerEventStore,
                                    BusinessWorkflows workflows) : base(store)
        {
            _logger = logger;
            this.workflows = workflows;
            this.customerStore = customerStore;
            this.customerEventStore = customerEventStore;
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
            if (long.TryParse(id, out var businessId))
            {
                return await customerStore.Query(new EntityStoreQueryOptions<Customer>(p.Page, _ => _.BusinessMembership.BusinessId == businessId &&
                    (EF.Functions.Like(_.CommonId, $"%{q.Term}%") || EF.Functions.Like(_.Name, $"%{q.Term}%"))));
            }
            else
            {
                var results = new List<Customer>();
                return new Paginated<Customer>(results, 0, 0, 1);
            }
        }

        [HttpDelete("{id}/Members/{customerId}")]
        public async Task<Customer> RemoveBusinessMembership(string id, long customerId)
        {
            var business = await base.GetResource(id);
            var customer = await workflows.RemoveBusinessMembership(business, customerId);
            return customer;
        }

        [HttpPost("{id}/Members/")]
        public async Task<BusinessMembership> AddMember(string id, AddMemberDto memberDto)
        {
            var business = await base.GetResource(id);
            var customer = await customerStore.ReadFromCommonId(memberDto.CommonId);
            var membership = await workflows.AddToBusiness(business.CommonId, customer);
            return membership;
        }

        /// <summary>Returns a list of events for a given business.</summary>
        [HttpGet("{id}/events")]
        public async Task<Paginated<CustomerEvent>> GetEvents(string id, [FromQuery] PaginateRequest p)
        {
            var business = await base.GetResource(id);
            return await customerEventStore.ReadEventsForBusiness(p.Page, business);
        }
    }
}
