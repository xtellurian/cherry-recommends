
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Core.Integrations;
using SignalBox.Infrastructure;

namespace SignalBox.Web.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("0.1")]
    public abstract class HubspotConnectorControllerBase : SignalBoxControllerBase
    {
        private readonly ILogger<HubspotConnectorControllerBase> logger;
        private readonly IHasher hasher;
        private readonly HubspotAppCredentials credentials;
        private readonly DeploymentInformation deploymentOptions;

        public HubspotConnectorControllerBase(ILogger<HubspotConnectorControllerBase> logger,
                                         IOptions<DeploymentInformation> deploymentOptions,
                                         IHasher hasher,
                                         IOptions<HubspotAppCredentials> hubspotOptions)
        {
            this.logger = logger;
            this.hasher = hasher;
            this.credentials = hubspotOptions.Value;
            this.deploymentOptions = deploymentOptions.Value;
        }

        protected async Task ValidateHubspotSignature(bool useRequestBody, string requestBody = null)
        {
            if (Request.Headers.TryGetValue("X-HubSpot-Signature-Version", out var sigVer))
            {
                logger.LogInformation($"X-HubSpot-Signature-Version: {sigVer}");
            }
            else
            {
                logger.LogWarning("X-HubSpot-Signature-Version header found ");
            }
            if (Request.Headers.TryGetValue("X-HubSpot-Signature", out var sig))
            {
                //https://legacydocs.hubspot.com/docs/faq/v2-request-validation
                // var s = "yyyyyyyy-yyyy-yyyy-yyyy-yyyyyyyyyyyyPOSThttps://www.example.com/webhook_uri{\"example_field\":\"サンプルデータ\"}";
                var sigValue = sig.ToString();
                var uri = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
                // need to include the body?
                // string valueToHash;
                string hashed;

                if (useRequestBody)
                {
                    requestBody ??= await Request.GetRawBodyStringAsync();
                    hashed = hasher.HashHttpRequestForWebhookValidation(credentials.ClientSecret, requestBody);
                }
                else
                {
                    hashed = hasher.HashHttpRequestForWebhookValidation(credentials.ClientSecret, Request.Method, uri);
                }

                var isValid = string.Equals(hashed, sigValue);

                if (!isValid)
                {
                    throw new BadRequestException("Hubspot Request was not signed correctly.");
                }
                else
                {
                    logger.LogInformation("X-HubSpot-Signature header matches computed value.");
                }
            }
            else
            {
                if (this.deploymentOptions?.Environment?.ToLower() == "production")
                {
                    throw new BadRequestException("X-HubSpot-Signature did not match computed signature");
                }
            }
        }
    }
}