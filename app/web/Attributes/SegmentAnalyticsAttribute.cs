using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using SignalBox.Core;
using SignalBox.Infrastructure;

namespace SignalBox.Web
{
    /// <summary>
    /// Attribute for sending events to Segment
    /// </summary>
    public class SegmentAnalyticsAttribute : TypeFilterAttribute
    {
        public SegmentAnalyticsAttribute()
            : base(typeof(SegmentAnalyticsActionFilter)) { }

        private class SegmentAnalyticsActionFilter : IActionFilter
        {
            public SegmentAnalyticsActionFilter(IConfiguration configuration, ITenantProvider tenantProvider)
            {
                Configuration = configuration;
                TenantProvider = tenantProvider;
            }

            public IConfiguration Configuration { get; }
            public ITenantProvider TenantProvider { get; }

            public void OnActionExecuted(ActionExecutedContext context)
            {

            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                if (context.Filters.Any(f => f.GetType() == typeof(SkipSegmentAnalyticsAttribute)))
                {
                    return;
                }

                string writeKey = Configuration.GetValue<string>("Segment:WriteKey");
                string userId = context.HttpContext.User.Auth0Id();
                string httpMethod = context.HttpContext.Request.Method;
                string eventName = $"[{httpMethod}]{context.ActionDescriptor.DisplayName}";
                var properties = new Dictionary<string, object>();
                
                string tenant = TenantProvider.Current()?.Name;

                properties.Add("tenant", tenant);

                if (!string.IsNullOrEmpty(writeKey))
                    Segment.Analytics.Client.Track(userId, eventName, properties);
            }
        }
    }
}