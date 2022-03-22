using System.Collections.Generic;
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
        private readonly ISegmentStore segmentStore;

        public CustomersController(ILogger<CustomersController> logger,
                                      IDateTimeProvider dateTimeProvider,
                                      CustomerWorkflows workflows,
                                      ICustomerStore store,
                                      ICustomerEventStore eventStore,
                                      ISegmentStore segmentStore) : base(store)
        {
            _logger = logger;
            this.dateTimeProvider = dateTimeProvider;
            this.workflows = workflows;
            this.eventStore = eventStore;
            this.segmentStore = segmentStore;
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
            return await eventStore.ReadEventsForUser(p, customer);
        }

        /// <summary>Returns a list of segments for a given user.</summary>
        [HttpGet("{id}/segments")]
        public async Task<IEnumerable<SignalBox.Core.Segment>> GetSegments(string id)
        {
            var customer = await this.GetResource(id);
            return await segmentStore.GetSegmentsByCustomer(customer);
        }

        /// <summary> Updates the properties of a customer.</summary>
        [HttpPost("{id}/properties")]
        public override async Task<DynamicPropertyDictionary> SetProperties(string id, [FromBody] DynamicPropertyDictionary properties, bool? useInternalId = null)
        {
            var customer = await base.GetResource(id);
            customer = await workflows.MergeUpdateProperties(customer, properties, saveOnComplete: true);
            return customer.Properties;
        }

        /// <summary>Adds a new Customer.</summary>
        [HttpPost]
        public async Task<object> CreateOrUpdate([FromBody] CreateOrUpdateCustomerDto dto)
        {
            return await workflows.CreateOrUpdate(new PendingCustomer(dto.GetCustomerId())
            {
                Name = dto.Name,
                Email = dto.Email,
                Properties = dto.Properties,
                IntegratedSystemReference = dto.IntegratedSystemReference
            });
        }

        /// <summary>Creates or updates a set of users with properties.</summary>
        [HttpPut]
        public async Task<object> CreateBatch([FromBody] BatchCreateOrUpdateCustomersDto dto)
        {
            await workflows.CreateOrUpdate(
                dto.Items().Select(u => new PendingCustomer(u.GetCustomerId())
                {
                    Name = u.Name,
                    Email = u.Email,
                    Properties = u.Properties,
                    IntegratedSystemReference = u.IntegratedSystemReference
                }
            ));
            return new object();
        }

        protected override Task<(bool, string)> CanDelete(Customer entity)
        {
            return Task.FromResult((true, ""));
        }
    }
}
