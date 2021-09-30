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
    public class EFParameterSetRecommenderStore : EFRecommenderStoreBase<ParameterSetRecommender>, IParameterSetRecommenderStore
    {
        public EFParameterSetRecommenderStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentService environmentService)
        : base(contextProvider, environmentService, (c) => c.ParameterSetRecommenders)
        { }

        public override async Task<ParameterSetRecommender> Read(long id)
        {
            try
            {
                return await Set
                    .Include(_ => _.Parameters)
                    .Include(_ => _.ModelRegistration)
                    .SingleAsync(_ => _.Id == id);
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException(typeof(ParameterSetRecommender), id, ex);
            }
        }

        public override async Task<ParameterSetRecommender> ReadFromCommonId(string commonId)
        {
            try
            {
                return await Set
                    .Include(_ => _.Parameters)
                    .Include(_ => _.ModelRegistration)
                    .SingleAsync(_ => _.CommonId == commonId);
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException(typeof(ParameterSetRecommender), commonId, ex);
            }
        }

        public override async Task<Paginated<TrackedUserAction>> QueryAssociatedActions(ParameterSetRecommender recommender,
                                                                                        int page,
                                                                                        bool revenueOnly)
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