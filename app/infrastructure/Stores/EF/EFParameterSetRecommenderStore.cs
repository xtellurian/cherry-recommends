using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFParameterSetRecommenderStore : EFRecommenderStoreBase<ParameterSetRecommender>, IParameterSetRecommenderStore
    {
        public EFParameterSetRecommenderStore(SignalBoxDbContext context)
        : base(context, (c) => c.ParameterSetRecommenders)
        {
        }

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
    }
}