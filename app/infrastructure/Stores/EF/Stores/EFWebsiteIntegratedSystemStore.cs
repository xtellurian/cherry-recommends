using SignalBox.Core;
using SignalBox.Core.Integrations.Website;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFWebsiteIntegratedSystemStore : EFCommonEntityStoreBase<WebsiteIntegratedSystem>, IWebsiteIntegratedSystemStore
    {
        protected override bool IsEnvironmentScoped => true;
        public EFWebsiteIntegratedSystemStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentService)
        : base(contextProvider, environmentService, c => c.WebsiteIntegratedSystems)
        { }

    }
}