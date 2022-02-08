using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class HistoricCustomerMetric : CustomerMetricBase
    {
        protected HistoricCustomerMetric()
        { }

        public static HistoricCustomerMetric FromString(Customer customer, Metric metric, string value, int version)
        {
            return new HistoricCustomerMetric(customer, metric, value, version);
        }

        public static HistoricCustomerMetric FromDouble(Customer customer, Metric metric, double value, int version)
        {
            return new HistoricCustomerMetric(customer, metric, value, version);
        }

        protected HistoricCustomerMetric(Customer customer, Metric metric, string value, int version)
        : base(customer, metric, value)
        {
            Version = version;
        }

        protected HistoricCustomerMetric(Customer customer, Metric metric, double value, int version)
        : base(customer, metric, value)
        {
            Version = version;
        }

#nullable enable
        public int Version { get; set; }
    }
}