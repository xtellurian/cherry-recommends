using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IRecommenderMetricTriggersWorkflow
    {
        Task HandleMetricValue(HistoricCustomerMetric metricValue);
    }
}