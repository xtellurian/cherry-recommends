using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFWebhookChannelStore : EFEnvironmentScopedEntityStoreBase<WebhookChannel>, IWebhookChannelStore
    {
        protected override bool IsEnvironmentScoped => true;
        public EFWebhookChannelStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentProvider)
        : base(contextProvider, environmentProvider, (c) => c.WebhookChannels)
        { }
    }
}