using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFFeatureGeneratorStore : EFEntityStoreBase<FeatureGenerator>, IFeatureGeneratorStore
    {
        public EFFeatureGeneratorStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, (c) => c.FeatureGenerators)
        { }
    }
}