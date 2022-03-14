using System.Text.Json.Serialization;

namespace SignalBox.Core.Metrics
{
    public class BusinessMetricValue : MetricValueBase
    {
        protected BusinessMetricValue() { }

        public BusinessMetricValue(Business business, Metric metric, double value, int version) : base(metric, version, value)
        {
            if (metric.Scope != MetricScopes.Business)
            {
                throw new BadRequestException("Business Metric Values can only be created for metrics with Business scope.");
            }

            Business = business;
            Version = version;
        }

        public BusinessMetricValue(Business business, Metric metric, string value, int version) : base(metric, version, value)
        {
            if (metric.Scope != MetricScopes.Business)
            {
                throw new BadRequestException("Business Metric Values can only be created for metrics with Business scope.");
            }

            Version = version;
            Business = business;
        }

        public long BusinessId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Business Business { get; set; }
    }
}
