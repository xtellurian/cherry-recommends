using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SignalBox.Core.Workflows;

namespace SignalBox.Web
{
    public class ApiKeyAuthenticationFilter : System.Attribute, IAuthorizationFilter
    {
        private readonly ApiKeyWorkflows workflows;

        public ApiKeyAuthenticationFilter(ApiKeyWorkflows workflows)
        {
            this.workflows = workflows;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            if (!context.HttpContext.Request.Query.TryGetValue("apiKey", out var k))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}