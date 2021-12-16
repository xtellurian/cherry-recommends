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
    [Route("api/TrackedUsers")]
    [Route("api/[controller]")]
    public class CustomersController : CommonEntityControllerBase<Customer>
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly CustomerWorkflows workflows;
        private readonly ICustomerEventStore eventStore;

        public CustomersController(ILogger<CustomersController> logger,
                                      IDateTimeProvider dateTimeProvider,
                                      CustomerWorkflows workflows,
                                      ICustomerStore store,
                                      ICustomerEventStore eventStore) : base(store)
        {
            _logger = logger;
            this.dateTimeProvider = dateTimeProvider;
            this.workflows = workflows;
            this.eventStore = eventStore;
        }

        public override async Task<Customer> GetResource(string id, bool? useInternalId = null)
        {
            if ((useInternalId == null || useInternalId == true) && long.TryParse(id, out var internalId))
            {
                return await store.Read(internalId, _ => _.IntegratedSystemMaps);
            }
            else if (useInternalId == true)
            {
                throw new BadRequestException("Internal Ids must be long integers");
            }
            else
            {
                return await store.ReadFromCommonId(id);
            }
        }

        /// <summary>Returns a list of events for a given user.</summary>
        [HttpGet("{id}/events")]
        public async Task<Paginated<CustomerEvent>> GetEvents([FromQuery] PaginateRequest p, string id, bool? useInternalId = null)
        {
            var customer = await store.GetEntity(id, useInternalId);
            return await eventStore.ReadEventsForUser(p.Page, customer);
        }

        /// <summary> Updates the properties of a customer.</summary>
        [HttpPost("{id}/properties")]
        public override async Task<DynamicPropertyDictionary> SetProperties(string id, [FromBody] DynamicPropertyDictionary properties, bool? useInternalId = null)
        {
            var customer = await base.GetResource(id);
            customer = await workflows.MergeUpdateProperties(customer, properties, null, saveOnComplete: true);
            return customer.Properties;
        }

        /// <summary>Adds a new Customer.</summary>
        [HttpPost]
        public async Task<object> CreateOrUpdate([FromBody] CreateOrUpdateCustomerDto dto)
        {
            return await workflows.CreateOrUpdate(dto.GetCustomerId(), dto.Name, dto.Properties,
                                                     dto.IntegratedSystemReference?.IntegratedSystemId,
                                                     dto.IntegratedSystemReference?.UserId);
        }

        /// <summary>Creates or updates a set of users with properties.</summary>
        [HttpPut]
        public async Task<object> CreateBatch([FromBody] BatchCreateOrUpdateCustomersDto dto)
        {
            await workflows.CreateOrUpdateMultiple(
                dto.Items().Select(u => new CustomerWorkflows.CreateOrUpdateCustomerModel(u.GetCustomerId(),
                                                                                         u.Name,
                                                                                         u.Properties,
                                                                                         u.IntegratedSystemReference?.IntegratedSystemId,
                                                                                         u.IntegratedSystemReference?.UserId)));
            return new object();
        }

        protected override Task<(bool, string)> CanDelete(Customer entity)
        {
            return Task.FromResult((true, ""));
        }
    }
}
