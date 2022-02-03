using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
            private readonly ILogger<SegmentAnalyticsActionFilter> logger;

            public SegmentAnalyticsActionFilter(IConfiguration configuration,
                                                ITenantProvider tenantProvider,
                                                ILogger<SegmentAnalyticsActionFilter> logger)
            {
                Configuration = configuration;
                TenantProvider = tenantProvider;
                this.logger = logger;
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
                if (!string.IsNullOrEmpty(writeKey))
                {
                    string httpMethod = context.HttpContext.Request.Method;
                    string eventName = $"[{httpMethod}]{context.ActionDescriptor.DisplayName}";
                    string tenant = TenantProvider.Current()?.Name;
                    var properties = new Dictionary<string, object>
                    {
                        { "tenant", tenant }
                    };

                    try
                    {
                        if (context.HttpContext.User.Identity.IsAuthenticated)
                        {
                            string userId = context.HttpContext.User.Auth0Id();
                            Segment.Analytics.Client.Track(userId, eventName, properties);
                        }
                        else
                        {
                            Segment.Analytics.Client.Track(null, eventName, properties,
                                new Segment.Model.Options()
                                    .SetAnonymousId(context.HttpContext.Session?.Id ?? System.Guid.NewGuid().ToString()));
                        }
                    }
                    catch (System.Exception ex)
                    {
                        logger.LogError("Segment tracking failed. {message}", ex.Message);
                    }
                }
            }
        }
    }
}