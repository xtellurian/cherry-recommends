using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFWebhookChannelStore : EFEntityStoreBase<WebhookChannel>, IWebhookChannelStore
    {
        public EFWebhookChannelStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, (c) => c.WebhookChannels)
        { }
    }
}