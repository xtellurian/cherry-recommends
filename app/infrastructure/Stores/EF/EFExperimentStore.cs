using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure
{
    public class EFExperimentStore : EFEntityStoreBase<Experiment>, IExperimentStore
    {
        public EFExperimentStore(SignalBoxDbContext context)
        : base(context, (c) => c.Experiments)
        {
        }

        public override async Task<Experiment> Read(long id)
        {
            return await Set
                .Include(_ => _.Offers)
                .Include(_ => _.Iterations)
                .SingleAsync(_ => _.Id == id);
        }
    }
}