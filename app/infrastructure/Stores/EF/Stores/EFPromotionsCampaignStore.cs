using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;
using SignalBox.Core.Campaigns;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFPromotionsCampaignStore : EFCampaignStoreBase<PromotionsCampaign>, IPromotionsCampaignStore
    {
        public EFPromotionsCampaignStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentService)
        : base(contextProvider, environmentService, (c) => c.PromotionsCampaigns)
        { }

        public override async Task<PromotionsCampaign> Read(long id, EntityStoreReadOptions options = null)
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
                throw new EntityNotFoundException(typeof(PromotionsCampaign), id, ex);
            }
        }

        public override async Task<PromotionsCampaign> ReadFromCommonId(string commonId)
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
                throw new EntityNotFoundException(typeof(PromotionsCampaign), commonId, ex);
            }
        }
    }
}