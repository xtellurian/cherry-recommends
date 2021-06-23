using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFParameterStore : EFCommonEntityStoreBase<Parameter>, IParameterStore
    {
        public EFParameterStore(SignalBoxDbContext context)
        : base(context, (c) => c.Parameters)
        {
        }
    }
}