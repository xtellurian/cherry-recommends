using System.Threading.Tasks;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core.Workflows
{
    public class RecommenderReportImageWorkflows : IRecommenderReportImageWorkflow, IWorkflow
    {
        private readonly IRecommenderImageFileStore fileStore;
        private readonly ITenantProvider tenantProvider;

        public RecommenderReportImageWorkflows(IRecommenderImageFileStore fileStore, ITenantProvider tenantProvider)
        {
            this.fileStore = fileStore;
            this.tenantProvider = tenantProvider;
        }

        public async Task<byte[]> DownloadImage(CampaignEntityBase campaign)
        {
            var tenant = tenantProvider.Current();
            var name = $"{campaign.Id}.png";
            if (tenant == null)
            {
                return await fileStore.ReadAllBytes(name);
            }
            else
            {
                return await fileStore.ReadAllBytes($"{tenant.Name}/{name}");
            }
        }
    }
}