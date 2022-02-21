namespace SignalBox.Core
{
    public class MetricCustomerExport
    {
        public string CustomerId { get; set; }
        public string Email { get; set; }
        public string MetricName { get; set; }
        public object MetricValue { get; set; }
        public double Quartile { get; set; }
    }
}