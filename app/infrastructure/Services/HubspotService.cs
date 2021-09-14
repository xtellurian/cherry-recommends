using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using xtellurian.HubSpot.Events;
using xtellurian.HubSpot.OAuth;
using xtellurian.HubSpot.Tickets;

namespace SignalBox.Infrastructure.Services
{
    public class HubspotService : IHubspotService
    {
        private string contactObjectType => "CONTACT";
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
            // use this for debugging the calls
            // this.httpClient = new HttpClient(new HttpClientLoggingHandler(new HttpClientHandler(), logger));
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
            var details = System.Text.Json.JsonSerializer.Deserialize<HubspotAccountDetails>(await response.Content.ReadAsStringAsync());
            return details;
        }

        public async Task<IEnumerable<HubspotEvent>> GetContactEvents(IntegratedSystem system,
                                                                DateTimeOffset? occurredAfter,
                                                                DateTimeOffset? occurredBefore,
                                                                long? objectId,
                                                                int? limit)
        {
            AuthorizeHttpClient(system);
            var eventsClient = new EventsClient(httpClient);
            // eventsClient.JsonSerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
            eventsClient.JsonSerializerSettings.DateFormatString = @"yyyy-MM-dd";
            // eventsClient.JsonSerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            // var x = JsonConvert.SerializeObject(new { d = occurredAfter }, eventsClient.JsonSerializerSettings);
            // var y = JsonConvert.SerializeObject(new { d = occurredBefore }, eventsClient.JsonSerializerSettings);
            // IsoDateTimeConverter converter = new IsoDateTimeConverter
            // {
            //     DateTimeStyles = DateTimeStyles.AdjustToUniversal
            //     // DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ssK"
            //     // 2017-03-15T11:45:42Z
            // };
            // eventsClient.JsonSerializerSettings.Converters.Add(converter);

            var res = await eventsClient.EventsV3EventsAsync(occurredAfter?.ToString("yyyy-MM-dd"),
                                                             occurredBefore?.ToString("yyyy-MM-dd"),
                                                             objectType: contactObjectType,
                                                             objectId: objectId,
                                                             eventType: null,
                                                             after: null,
                                                             before: null,
                                                             limit: limit,
                                                             sort: null);

            return res.Results.Select(_ => new HubspotEvent(_.ObjectId, _.ObjectType, DateTimeOffset.Parse(_.OccurredAt), _.Properties));
        }

        public async Task<IEnumerable<HubspotContactProperty>> GetContactProperties(IntegratedSystem system)
        {
            AuthorizeHttpClient(system);
            var contactPropertiesClient = new ContactPropertiesClient(httpClient);

            var res = await contactPropertiesClient.CrmV3PropertiesGetAsync(contactObjectType, false);
            return res.Results.Select(_ => new HubspotContactProperty(_.Name, _.Label, _.Type, _.Description, _.HubspotDefined));
        }

        public async Task<Paginated<HubspotContact>> GetContacts(IntegratedSystem system, string after = null, IEnumerable<string> properties = null)
        {
            AuthorizeHttpClient(system);
            var contactsClient = new ContactsClient(httpClient);

            var res = await contactsClient.CrmV3ObjectsContactsGetAsync(10, after, properties: properties, associations: null, archived: false);
            // res.Paging.Next.After

            var items = res.Results.Select(_ => new HubspotContact(_.Id, _.Properties));
            return new Paginated<HubspotContact>(items, res.Paging?.Next?.After);
        }

        public async Task<HubspotContact> GetContact(IntegratedSystem system, string contactId, IEnumerable<string> properties = null)
        {
            AuthorizeHttpClient(system);
            var contactsClient = new ContactsClient(httpClient);
            var res = await contactsClient.CrmV3ObjectsContactsGetAsync(contactId, properties, associations: null, archived: null, idProperty: null);
            return new HubspotContact(res.Id, res.Properties);
        }

        public async Task<IEnumerable<HubspotAssociation>> GetAssociatedContactsFromTicket(IntegratedSystem system, string ticketId)
        {
            AuthorizeHttpClient(system);
            var ticketsClient = new TicketsClient(httpClient);
            try
            {
                var associations = await ticketsClient.CrmV3ObjectsTicketsAssociationsGetAsync(ticketId, "CONTACT", null, null);
                logger.LogInformation($"Found {associations.Results.Count} associations");
                return associations.Results.Select(_ => new HubspotAssociation(_.Id, _.Type));
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex.Message);
                throw new IntegratedSystemException("Failed to get associated object", ex);
            }

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

