using System.Threading.Tasks;
using SignalBox.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFSegmentStore : EFEntityStoreBase<Segment>, ISegmentStore
    {
        public EFSegmentStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, (c) => c.Segments)
        { }

        public async Task<bool> CustomerExistsInSegment(Segment segment, long customerId)
        {
            return await context.CustomerSegments.AnyAsync(_ => _.SegmentId == segment.Id && _.CustomerId == customerId);
        }

        public async Task<bool> RecommenderHasSegmentInAudience(Segment segment, long recommenderId)
        {
            return await context.RecommenderSegments.AnyAsync(_ => _.SegmentId == segment.Id && _.RecommenderId == recommenderId);
        }

        public async Task<CustomerSegment> AddCustomer(Segment segment, Customer customer)
        {
            CustomerSegment customerSegment = new CustomerSegment(customer, segment);

            if (!await CustomerExistsInSegment(segment, customer.Id))
            {
                await context.CustomerSegments.AddAsync(customerSegment);
            }

            return customerSegment;
        }

        public async Task<CustomerSegment> RemoveCustomer(Segment segment, Customer customer)
        {
            CustomerSegment customerSegment = await context.CustomerSegments.FirstOrDefaultAsync(_ => _.SegmentId == segment.Id && _.CustomerId == customer.Id);

            if (customerSegment != null)
            {
                context.CustomerSegments.Remove(customerSegment);
            }

            return customerSegment;
        }

        public async Task<IEnumerable<Segment>> GetSegmentsByCustomer(Customer customer)
        {
            return await QuerySet
                .Where(_ => _.InSegment.Any(cs => cs.CustomerId == customer.Id))
                .ToListAsync();
        }

        public async Task<RecommenderSegment> AddRecommender(Segment segment, RecommenderEntityBase recommender)
        {
            RecommenderSegment recommenderSegment = new RecommenderSegment(recommender, segment);

            if (!await RecommenderHasSegmentInAudience(segment, recommender.Id))
            {
                await context.RecommenderSegments.AddAsync(recommenderSegment);
            }

            return recommenderSegment;
        }

        public async Task<RecommenderSegment> RemoveRecommender(Segment segment, RecommenderEntityBase recommender)
        {
            RecommenderSegment recommenderSegment = await context.RecommenderSegments.FirstOrDefaultAsync(_ => _.SegmentId == segment.Id && _.RecommenderId == recommender.Id);

            if (recommenderSegment != null)
            {
                context.RecommenderSegments.Remove(recommenderSegment);
            }

            return recommenderSegment;
        }

        public async Task<IEnumerable<Segment>> GetSegmentsByRecommender(RecommenderEntityBase recommender)
        {
            return await QuerySet
                .Where(_ => _.RecommenderSegments.Any(cs => cs.RecommenderId == recommender.Id))
                .ToListAsync();
        }
    }
}