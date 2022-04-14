using Microsoft.AspNetCore.Mvc.Filters;

namespace SignalBox.Web
{
    public class ContentSecurityPolicyAttribute : ResultFilterAttribute
    {
        private readonly string contentSecurityPolicy;

        public ContentSecurityPolicyAttribute(string contentSecurityPolicy)
        {
            this.contentSecurityPolicy = contentSecurityPolicy;
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add("Content-Security-Policy", contentSecurityPolicy);
            base.OnResultExecuting(context);
        }
    }
}
