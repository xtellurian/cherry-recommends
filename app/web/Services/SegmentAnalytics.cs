using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace SignalBox.Web.Services
{
    public static class SegmentAnalytics 
    {
        public static IServiceCollection UseSegmentAnalytics(this IServiceCollection services, string writeKey)
        {
            if (!string.IsNullOrEmpty(writeKey))
            {
                Segment.Analytics.Initialize(writeKey);
            }

            return services;
        }
    }
}