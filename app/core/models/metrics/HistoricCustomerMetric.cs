using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class HistoricCustomerMetric : CustomerMetricBase
    {
        protected HistoricCustomerMetric()
        { }

        public HistoricCustomerMetric(Customer customer, Metric metric, string value, int version)
        : base(customer, metric, value)
        {
            Version = version;
        }

        public HistoricCustomerMetric(Customer customer, Metric metric, double value, int version)
        : base(customer, metric, value)
        {
            Version = version;
        }

#nullable enable
        public int Version { get; set; }
    }
}