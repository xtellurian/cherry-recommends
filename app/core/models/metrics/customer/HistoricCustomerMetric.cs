using System.Text.Json.Serialization;
using SignalBox.Core.Metrics;

namespace SignalBox.Core
{
    public class HistoricCustomerMetric : MetricValueBase
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

        public HistoricCustomerMetric(Customer customer, Metric metric, string value, int version)
        : base(metric, version, value)
        {
            Version = version;
            Customer = customer;
        }
        public HistoricCustomerMetric(Customer customer, Metric metric, double value, int version)
      : base(metric, version, value)
        {
            Customer = customer;
            Version = version;
        }

        public long TrackedUserId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Customer TrackedUser => Customer;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Customer Customer { get; set; }
    }
}