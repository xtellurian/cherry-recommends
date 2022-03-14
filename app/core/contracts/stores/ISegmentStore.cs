using System.Collections.Generic;
using System.Threading.Tasks;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public interface ISegmentStore : IEntityStore<Segment>
    {
        Task<bool> CustomerExistsInSegment(Segment segment, long customerId);
        Task<bool> RecommenderHasSegmentInAudience(Segment segment, long recommenderId);
        Task<CustomerSegment> AddCustomer(Segment segment, Customer customer);
        Task<CustomerSegment> RemoveCustomer(Segment segment, Customer customer);
        Task<IEnumerable<Segment>> GetSegmentsByCustomer(Customer customer);
        Task<RecommenderSegment> AddRecommender(Segment segment, RecommenderEntityBase recommender);
        Task<RecommenderSegment> RemoveRecommender(Segment segment, RecommenderEntityBase recommender);
        Task<IEnumerable<Segment>> GetSegmentsByRecommender(RecommenderEntityBase recommender);
    }
}