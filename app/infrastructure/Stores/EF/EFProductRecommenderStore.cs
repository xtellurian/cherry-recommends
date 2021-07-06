using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFProductRecommenderStore : EFCommonEntityStoreBase<ProductRecommender>, IProductRecommenderStore
    {
        public EFProductRecommenderStore(SignalBoxDbContext context)
        : base(context, (c) => c.ProductRecommenders)
        {
        }

        public override async Task<ProductRecommender> Read(long id)
        {
            try
            {
                return await Set
                    .Include(_ => _.Products)
                    .Include(_ => _.ModelRegistration)
                    .Include(_ => _.Touchpoint)
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
                    .Include(_ => _.Touchpoint)
                    .FirstAsync(_ => _.CommonId == commonId);
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException(typeof(ProductRecommender), commonId, ex);
            }
        }
    }
}