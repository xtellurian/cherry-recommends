using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Infrastructure.Services
{
    // this is a helpder class for debugging or logging external calls
    internal class HttpClientLoggingHandler : DelegatingHandler
    {
        private readonly ILogger logger;

        public HttpClientLoggingHandler(HttpMessageHandler innerHandler, ILogger logger)
            : base(innerHandler)
        {
            this.logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            logger.LogDebug(request.RequestUri.ToString());
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
            var c = await response.Content.ReadAsStringAsync();
            return response;
        }
    }
}