using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFParameterSetRecommenderStore : EFCommonEntityStoreBase<ParameterSetRecommender>, IParameterSetRecommenderStore
    {
        public EFParameterSetRecommenderStore(SignalBoxDbContext context)
        : base(context, (c) => c.ParameterSetRecommenders)
        {
        }

        public override Task<ParameterSetRecommender> Read(long id)
        {
            // always include parameters
            return base.Read(id, _ => _.Parameters);
        }

        public override Task<ParameterSetRecommender> ReadFromCommonId(string commonId)
        {
            // always include the parameters
            return base.ReadFromCommonId(commonId, _ => _.Parameters);
        }
    }
}