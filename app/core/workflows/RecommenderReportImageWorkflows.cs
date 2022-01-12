using System.Threading.Tasks;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Workflows
{
    public class RecommenderReportImageWorkflows : IWorkflow
    {
        private readonly IRecommenderImageFileStore fileStore;
        private readonly ITenantProvider tenantProvider;

        public RecommenderReportImageWorkflows(IRecommenderImageFileStore fileStore, ITenantProvider tenantProvider)
        {
            this.fileStore = fileStore;
            this.tenantProvider = tenantProvider;
        }

        public async Task<byte[]> DownloadImage(RecommenderEntityBase recommender)
        {
            var tenant = tenantProvider.Current();
            var name = $"{recommender.Id}.png";
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