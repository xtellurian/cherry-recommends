using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SignalBox.Core.Adapters.Hubspot;
using SignalBox.Core.OAuth;

namespace SignalBox.Core
{
    public interface IHubspotService
    {
        Task<TokenResponse> ExchangeCode(string clientId, string clientSecret, string redirectUri, string code);
        Task<TokenResponse> UseRefreshToken(string clientId, string clientSecret, string refreshToken);
        Task<HubspotAccountDetails> GetAccountDetails(TokenResponse tokenResponse);
        Task<IEnumerable<HubspotContactProperty>> GetContactProperties(IntegratedSystem hubspotSystemReference);
        Task<Paginated<HubspotContact>> GetContacts(IntegratedSystem system, string after = null, IEnumerable<string> properties = null);
        Task<IEnumerable<HubspotAssociation>> GetAssociatedContactsFromTicket(IntegratedSystem system, string ticketId);
        Task<IEnumerable<HubspotEvent>> GetContactEvents(IntegratedSystem system,
                                                         DateTimeOffset? occurredAfter,
                                                         DateTimeOffset? occurredBefore,
                                                         long? objectId,
                                                         int? limit);
        Task<HubspotContact> GetContact(IntegratedSystem system, string contactId, IEnumerable<string> properties = null);
    }
}