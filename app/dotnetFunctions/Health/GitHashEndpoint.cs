using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using SignalBox.Infrastructure;

namespace dotnetFunctions
{
    public class GitHashEndpoint
    {
        [Function("GitHash")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            var hash = VersionInfo.GitHash ?? "null";
            response.WriteString(hash);
            return response;
        }
    }
}
