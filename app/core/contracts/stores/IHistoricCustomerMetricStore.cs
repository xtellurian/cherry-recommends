using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IHistoricCustomerMetricStore : IEntityStore<HistoricCustomerMetric>
    {
        /// <summary>
        /// If version = null, returns the latest value of a customer metric.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="metric"></param>
        /// <param name="version">If null, returns the latest value.</param>
        /// <returns></returns>
        Task<HistoricCustomerMetric> ReadCustomerMetric(Customer customer, Metric metric, int? version = null);
        Task<bool> MetricExists(Customer customer, Metric metric, int? version = null);
        Task<int> CurrentMaximumCustomerMetricVersion(Customer customer, Metric metric);
        Task<IEnumerable<Metric>> GetMetricsFor(Customer customer);
        Task<IEnumerable<CustomerMetricWeeklyNumericAggregate>> GetAggregateMetricValuesNumeric(Metric metric, int weeksAgo = 11);
        Task<IEnumerable<CustomerMetricWeeklyStringAggregate>> GetAggregateMetricValuesString(Metric metric, int weeksAgo = 11);
        Task<IEnumerable<MetricDailyBinValueNumeric>> GetMetricBinValuesNumeric(Metric metric, int? binCount = 12);
        Task<IEnumerable<MetricDailyBinValueString>> GetMetricBinValuesString(Metric metric, int topKValue = 12);
        Task<IEnumerable<MetricCustomerExport>> GetMetricCustomerExports(Metric metric);
        Task<LatestMetric> LatestCustomerMetricValue(Customer customer, Metric metric);
        IAsyncEnumerable<LatestMetric> IterateLatest(long metricId, Expression<Func<LatestMetric, bool>> predicate = null);
    }
}