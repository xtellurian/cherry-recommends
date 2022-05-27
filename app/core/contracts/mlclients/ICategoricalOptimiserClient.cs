using System.Threading.Tasks;
using SignalBox.Core.Optimisers;

namespace SignalBox.Core
{
    public interface ICategoricalOptimiserClient
    {
        Task<CategoricalOptimiser> Create(ICampaign campaign);
    }
}