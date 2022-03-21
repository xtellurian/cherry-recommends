using System.Threading.Tasks;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public interface IRecommenderReportImageWorkflow
    {
        Task<byte[]> DownloadImage(RecommenderEntityBase recommender);
    }
}