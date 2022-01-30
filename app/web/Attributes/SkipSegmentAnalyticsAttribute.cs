
using Microsoft.AspNetCore.Mvc.Filters;

namespace SignalBox.Web
{
    /// <summary>
    /// Marker attribute to skip Segment Analytics filter
    /// </summary>
    public class SkipSegmentAnalyticsAttribute : ActionFilterAttribute
    {

    }
}