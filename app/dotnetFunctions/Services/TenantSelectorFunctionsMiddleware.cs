using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Infrastructure.Models;

namespace SignalBox.Functions
{
    public class TenantSelectorFunctionsMiddleware : IFunctionsWorkerMiddleware
    {

        private readonly ITenantStore tenantStore;
        private readonly Hosting hosting;

        public TenantSelectorFunctionsMiddleware(ITenantStore tenantStore, IOptions<Hosting> hostingOptions)
        {
            this.tenantStore = tenantStore;
            this.hosting = hostingOptions.Value;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var tenantProvider = (ITenantProvider)context.InstanceServices.GetService(typeof(ITenantProvider));
            var queryDic = context.BindingContext.BindingData["Query"] as string;
            if (hosting.Multitenant)
            {
                if (!string.IsNullOrEmpty(queryDic))
                {
                    var dic = JsonSerializer.Deserialize<Dictionary<string, string>>(queryDic);
                    if (dic.ContainsKey("tenant"))
                    {
                        await tenantProvider.SetTenantName(dic["tenant"]);
                    }
                }
            }

            await next(context);
        }
    }
}