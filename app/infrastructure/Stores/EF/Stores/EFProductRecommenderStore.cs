using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFProductRecommenderStore : EFRecommenderStoreBase<ProductRecommender>, IProductRecommenderStore
    {
        public EFProductRecommenderStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentService environmentService)
        : base(contextProvider, environmentService, (c) => c.ProductRecommenders)
        { }

        public override async Task<ProductRecommender> Read(long id)
        {
            try
            {
                return await Set
                    .Include(_ => _.Products)
                    .Include(_ => _.ModelRegistration)
                    .FirstAsync(_ => _.Id == id);
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException(typeof(ProductRecommender), id, ex);
            }
        }

        public override async Task<ProductRecommender> ReadFromCommonId(string commonId)
        {
            try
            {
                return await Set
                    .Include(_ => _.Products)
                    .Include(_ => _.ModelRegistration)
                    .FirstAsync(_ => _.CommonId == commonId);
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException(typeof(ProductRecommender), commonId, ex);
            }
        }

        public override async Task<Paginated<TrackedUserAction>> QueryAssociatedActions(ProductRecommender recommender, int page, bool revenueOnly)
        {
            Expression<Func<TrackedUserAction, bool>> actionFilter = _ => true;
            if (revenueOnly)
            {
                actionFilter = _ => _.AssociatedRevenue != null;
            }
            var itemCount = await context.RecommendationCorrelators
                .Where(_ => _.RecommenderId == recommender.Id)
                .SelectMany(_ => _.TrackedUserActions)
                .Where(actionFilter)
                .CountAsync();

            List<TrackedUserAction> results;

            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await context.RecommendationCorrelators
                    .Where(_ => _.RecommenderId == recommender.Id)
                    .SelectMany(_ => _.TrackedUserActions)
                    .Where(actionFilter)
                    .OrderByDescending(_ => _.Timestamp)
                    .Skip((page - 1) * PageSize).Take(PageSize)
                    .ToListAsync();
            }
            else
            {
                results = new List<TrackedUserAction>();
            }
            var pageCount = (int)Math.Ceiling((double)itemCount / PageSize);
            return new Paginated<TrackedUserAction>(results, pageCount, itemCount, page);
        }
    }
}