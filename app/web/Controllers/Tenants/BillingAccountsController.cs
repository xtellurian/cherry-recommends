
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalBox.Core;
using SignalBox.Core.Accounts;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Tenants/{id:int}/Account")]
    public class BillingAccountsController : SignalBoxControllerBase
    {
        private readonly ITenantStore tenantStore;
        private readonly ITenantProvider tenantProvider;

        public BillingAccountsController(ITenantStore tenantStore, ITenantProvider tenantProvider)
        {
            this.tenantStore = tenantStore;
            this.tenantProvider = tenantProvider;
        }

        [HttpGet]
        public async Task<BillingAccount> GetBillingAccount(long id)
        {
            var currentTenant = tenantProvider.Current();
            if (currentTenant.Id != id)
            {
                throw new BadRequestException("Cannot access account of another tenant");
            }

            await tenantStore.Load(currentTenant, _ => _.Account);
            return currentTenant.Account ?? new BillingAccount(currentTenant, PlanTypes.None);
        }
    }
}
