using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;
using SignalBox.Core.Campaigns;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFParameterSetCampaignStore : EFCampaignStoreBase<ParameterSetCampaign>, IParameterSetCampaignStore
    {
        public EFParameterSetCampaignStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentService)
        : base(contextProvider, environmentService, (c) => c.ParameterSetCampaigns)
        { }

        public override async Task<ParameterSetCampaign> Read(long id)
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
                throw new EntityNotFoundException(typeof(ParameterSetCampaign), id, ex);
            }
        }

        public override async Task<ParameterSetCampaign> ReadFromCommonId(string commonId)
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
                throw new EntityNotFoundException(typeof(ParameterSetCampaign), commonId, ex);
            }
        }
    }
}