using System.Threading.Tasks;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core
{
    public interface IRecommenderReportImageWorkflow
    {
        Task<byte[]> DownloadImage(CampaignEntityBase recommender);
    }
}