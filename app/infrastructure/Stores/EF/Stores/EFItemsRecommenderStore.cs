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
    public class EFItemsRecommenderStore : EFRecommenderStoreBase<ItemsRecommender>, IItemsRecommenderStore
    {
        public EFItemsRecommenderStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentService)
        : base(contextProvider, environmentService, (c) => c.ItemsRecommenders)
        { }

        public override async Task<ItemsRecommender> Read(long id)
        {
            try
            {
                return await QuerySet
                    .Include(_ => _.Items)
                    .Include(_ => _.ModelRegistration)
                    .FirstAsync(_ => _.Id == id);
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException(typeof(ItemsRecommender), id, ex);
            }
        }

        public override async Task<ItemsRecommender> ReadFromCommonId(string commonId)
        {
            try
            {
                return await QuerySet
                    .Include(_ => _.Items)
                    .Include(_ => _.ModelRegistration)
                    .FirstAsync(_ => _.CommonId == commonId);
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException(typeof(ItemsRecommender), commonId, ex);
            }
        }
    }
}