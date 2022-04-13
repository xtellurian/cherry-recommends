using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalBox.Core;

namespace SignalBox.Web.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [SegmentAnalytics]
    [TypeFilter(typeof(SecurityExceptionFilter), IsReusable = true)]
    public class SignalBoxControllerBase : ControllerBase
    {
        protected async Task<Customer> LoadCustomer(ICustomerStore customerStore,
                                                          string internalOrCommonId,
                                                          bool? useInternalId = null)
        {
            return await customerStore.GetEntity<Customer>(internalOrCommonId, useInternalId);
        }
    }
}