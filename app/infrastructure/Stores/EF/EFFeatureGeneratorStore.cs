using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFFeatureGeneratorStore : EFEntityStoreBase<FeatureGenerator>, IFeatureGeneratorStore
    {
        public EFFeatureGeneratorStore(SignalBoxDbContext context)
        : base(context, (c) => c.FeatureGenerators)
        {
        }
    }
}