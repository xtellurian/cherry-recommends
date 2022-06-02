using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            private readonly IOptionsMonitor<SegmentConfig> segmentOptions;
            private readonly ITenantProvider tenantProvider;
            private readonly IOptionsMonitor<DeploymentInformation> diOptions;
            private readonly ILogger<SegmentAnalyticsActionFilter> logger;

            public SegmentAnalyticsActionFilter(IOptionsMonitor<SegmentConfig> segmentOptions, ITenantProvider tenantProvider, IOptionsMonitor<DeploymentInformation> diOptions, ILogger<SegmentAnalyticsActionFilter> logger)
            {
                this.segmentOptions = segmentOptions;
                this.tenantProvider = tenantProvider;
                this.diOptions = diOptions;
                this.logger = logger;
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {

            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                if (context.Filters.Any(f => f.GetType() == typeof(SkipSegmentAnalyticsAttribute)))
                {
                    // don't track explicitly filtered endpoints
                    return;
                }

                if (context.ActionDescriptor.EndpointMetadata.Any(f => f.GetType() == typeof(AllowApiKeyAttribute)))
                {
                    // don't track if the endpoint allows an API key. not a user action.
                    return;
                }

                string httpMethod = context.HttpContext.Request.Method;
                if (string.Equals(httpMethod, "GET", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    // don't track GET. It happens too often and isn't useful
                    return;
                }

                try
                {
                    var segmentConfig = segmentOptions.CurrentValue;
                    string writeKey = segmentConfig?.WriteKey;
                    if (!string.IsNullOrEmpty(writeKey))
                    {
                        // remove prefix Signalbox.Web.Controllers. - it's on everything
                        string eventName = $"[{httpMethod}]{context.ActionDescriptor.DisplayName.Replace("Signalbox.Web.Controllers.", "")}";
                        string tenant = tenantProvider.Current()?.Name;
                        string stack = diOptions.CurrentValue.Stack;
                        var properties = new Dictionary<string, object>
                        {
                            { "tenant", tenant },
                            { "stack", stack },
                        };

                        if (context.HttpContext.User.Identity.IsAuthenticated)
                        {
                            string userId = context.HttpContext.User.Auth0Id();
                            properties["email"] = context.HttpContext.User.Email();
                            Segment.Analytics.Client.Track(userId, eventName, properties);
                        }
                        else
                        {
                            Segment.Analytics.Client.Track(null, eventName, properties,
                                new Segment.Model.Options()
                                    .SetAnonymousId(context.HttpContext.Session?.Id ?? System.Guid.NewGuid().ToString()));
                        }
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