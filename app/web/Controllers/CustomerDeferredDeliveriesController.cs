using System.Collections.Generic;
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
    [Route("api/Customers/{id}/DeferredDeliveries")]
    public class CustomerDeferredDeliveriesController : SignalBoxControllerBase
    {
        private readonly ICustomerStore customerStore;
        private readonly IDeferredDeliveryStore deferredDeliveryStore;

        public CustomerDeferredDeliveriesController(ICustomerStore customerStore,
                                                    IDeferredDeliveryStore deferredDeliveryStore)
        {
            this.customerStore = customerStore;
            this.deferredDeliveryStore = deferredDeliveryStore;
        }

        /// <summary>Gets recent recommendations for a business.</summary>
        [HttpGet]
        public async Task<IEnumerable<DeferredDelivery>> GetDeferredDeliveries([FromQuery] PaginateRequest p, string id, bool? useInternalId = null)
        {
            var customer = await customerStore.GetEntity(id, useInternalId);
            return await deferredDeliveryStore.QueryForCustomer(customer.Id);
        }
    }
}
