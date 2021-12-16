using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IFeatureStore : ICommonEntityStore<Feature>
    {
        Task<Paginated<Customer>> QueryCustomers(int page, long featureId);
    }
}