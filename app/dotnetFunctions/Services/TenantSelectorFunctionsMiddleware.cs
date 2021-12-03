using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Infrastructure.Models;

namespace SignalBox.Functions
{
    public class TenantSelectorFunctionsMiddleware : IFunctionsWorkerMiddleware
    {

        private readonly ITenantStore tenantStore;
        private readonly ILogger<TenantSelectorFunctionsMiddleware> logger;
        private readonly Hosting hosting;

        public TenantSelectorFunctionsMiddleware(ITenantStore tenantStore,
                                                 ILogger<TenantSelectorFunctionsMiddleware> logger,
                                                 IOptions<Hosting> hostingOptions)
        {
            this.tenantStore = tenantStore;
            this.logger = logger;
            this.hosting = hostingOptions.Value;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var tenantProvider = (ITenantProvider)context.InstanceServices.GetService(typeof(ITenantProvider));

            if (context.BindingContext.BindingData.TryGetValue("TenantName", out var tenantNameObj))
            {
                if (tenantNameObj != null)
                {
                    await tenantProvider.SetTenantName(tenantNameObj?.ToString());
                }
            }
            else if (context.BindingContext.BindingData.TryGetValue("Query", out var queryDicObject))
            {
                var queryDic = queryDicObject as string;
                if (hosting.Multitenant)
                {
                    if (!string.IsNullOrEmpty(queryDic))
                    {
                        var dic = JsonSerializer.Deserialize<Dictionary<string, string>>(queryDic);
                        if (dic.ContainsKey("tenant"))
                        {
                            var tenantName = dic["tenant"];
                            logger.LogInformation($"Setting tenant to {tenantName} from query string");
                            await tenantProvider.SetTenantName(tenantName);
                        }
                    }
                }
            }

            await next(context);
        }
    }
}