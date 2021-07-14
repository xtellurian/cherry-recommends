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
        Task<IEnumerable<HubspotContact>> GetContacts(IntegratedSystem system);
        Task<IEnumerable<HubspotAssociation>> GetAssociatedContactsFromTicket(IntegratedSystem system, string ticketId);
    }
}