using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Core.Adapters.Hubspot;
using SignalBox.Core.Integrations;
using SignalBox.Core.OAuth;
using xtellurian.HubSpot.ContactProperties;
using xtellurian.HubSpot.Contacts;
using xtellurian.HubSpot.CrmExtensions;
using xtellurian.HubSpot.OAuth;

namespace SignalBox.Infrastructure.Services
{
    public class HubspotService : IHubspotService
    {
        private static List<string> properties = new List<string>
        {
            "hs_object_id",
            "hs_email_sends_since_last_engagement",
            "hs_analytics_average_page_views",
        };
        private readonly HttpClient httpClient;
        private readonly ILogger<HubspotService> logger;
        private readonly HubspotAppCredentials creds;

        public HubspotService(HttpClient httpClient, IOptions<HubspotAppCredentials> creds, ILogger<HubspotService> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.creds = creds.Value;
        }

        private void AuthorizeHttpClient(IntegratedSystem system)
        {
            if (system.SystemType != IntegratedSystemTypes.Hubspot)
            {
                throw new BadRequestException("Only Hubspot integrated systems can access HubSpot contact data");
            }

            SetAccessToken(system.TokenResponse);
        }

        private void SetAccessToken(TokenResponse tokenResponse)
        {
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
        }

        public async Task<TokenResponse> UseRefreshToken(string clientId, string clientSecret, string refreshToken)
        {
            var oauthClient = new OAuthClient(httpClient);
            var r = await oauthClient.UseRefreshToken(clientId, clientSecret, refreshToken);
            return new TokenResponse(r.AccessToken, r.RefreshToken, null, r.ExpiresIn);
        }

        public async Task<TokenResponse> ExchangeCode(string clientId, string clientSecret, string redirectUri, string code)
        {
            var oauthClient = new OAuthClient(httpClient);
            var r = await oauthClient.ExchangeCode(clientId, clientSecret, redirectUri, code);
            return new TokenResponse(r.AccessToken, r.RefreshToken, null, r.ExpiresIn);
        }

        public async Task<HubspotAccountDetails> GetAccountDetails(TokenResponse tokenResponse)
        {
            SetAccessToken(tokenResponse);
            var response = await httpClient.GetAsync("https://api.hubapi.com/integrations/v1/me");
            response.EnsureSuccessStatusCode();
            var details = JsonSerializer.Deserialize<HubspotAccountDetails>(await response.Content.ReadAsStringAsync());
            return details;
        }

        public async Task<IEnumerable<HubspotContactProperty>> GetContactProperties(IntegratedSystem system)
        {
            AuthorizeHttpClient(system);
            var contactPropertiesClient = new ContactPropertiesClient(httpClient);

            var res = await contactPropertiesClient.CrmV3PropertiesGetAsync("contact", false);
            return res.Results.Select(_ => new HubspotContactProperty(_.Name, _.Label, _.Type, _.Description, _.HubspotDefined));
        }

        public async Task<IEnumerable<HubspotContact>> GetContacts(IntegratedSystem system)
        {
            AuthorizeHttpClient(system);
            var contactsClient = new ContactsClient(httpClient);

            var res = await contactsClient.CrmV3ObjectsContactsGetAsync(10, null, null, null, false);
            // TODO: paging

            return res.Results.Select(_ => new HubspotContact(_.Id, _.Properties));
        }

        public async Task CreateCard(IntegratedSystem system)
        {
            AuthorizeHttpClient(system);
            var crmExtensionsClient = new CrmExtensionsClient(httpClient);

            try
            {


                var response = await crmExtensionsClient.CrmV3ExtensionsCardsPostAsync(int.Parse(creds.AppId), new CardCreateRequest
                {
                    Title = "Four2 Card Title - PET",
                    Fetch = new CardFetchBody
                    {
                        ObjectTypes = new List<CardObjectTypeBody>
                    {
                        new CardObjectTypeBody
                        {
                            Name = CardObjectTypeBodyName.Contacts,
                            PropertiesToSend = new List<string>
                            {
                                "email"
                            }
                        }
                    },
                        TargetUrl = "https://34491bc3e563.ngrok.io/api/hubspotintegratedsystems/1/cardcards/touchpintname"
                    },
                    Display = new CardDisplayBody
                    {
                        Properties = new List<CardDisplayProperty>
                    {
                        new CardDisplayProperty
                        {
                            DataType = CardDisplayPropertyDataType.STRING,
                            Name = "petName",
                            Label = "Pet Name"
                        }
                    }
                    }
                });

                logger.LogInformation("Created a card!");
            }
            catch (Exception ex)
            {
                logger.LogCritical("oops", ex);
            }
        }

    }
}

