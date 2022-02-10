using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFHistoricCustomerMetricStore : EFEntityStoreBase<HistoricCustomerMetric>, IHistoricCustomerMetricStore
    {
        public EFHistoricCustomerMetricStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, c => c.HistoricCustomerMetrics)
        { }

        public async Task<int> CurrentMaximumCustomerMetricVersion(Customer customer, Metric metric)
        {
            var latest = await context.LatestFeatureVersions
                .Where(_ => _.MetricId == metric.Id && _.CustomerId == customer.Id)
                .FirstOrDefaultAsync();
            return latest?.MaxVersion ?? 0;
        }

        public async Task<IEnumerable<Metric>> GetMetricsFor(Customer customer)
        {
            var features = await context.HistoricCustomerMetrics
                .Where(_ => _.TrackedUserId == customer.Id)
                .Include(_ => _.Metric)
                .Select(_ => _.Metric)
                .Distinct()
                .ToListAsync();

            return features;
        }

        public async Task<HistoricCustomerMetric> ReadCustomerMetric(Customer customer, Metric metric, int? version = null)
        {
            version ??= await CurrentMaximumCustomerMetricVersion(customer, metric);
            customer = await context.Customers
                .Include(_ => _.HistoricCustomerMetrics)
                .ThenInclude(_ => _.Metric)
                .FirstAsync(_ => _.Id == customer.Id);

            return customer.HistoricCustomerMetrics.First(_ => _.MetricId == metric.Id && _.Version == version.Value);
        }

        public async Task<bool> MetricExists(Customer customer, Metric metric, int? version = null)
        {
            version ??= await CurrentMaximumCustomerMetricVersion(customer, metric);
            customer = await context.Customers
                .Include(_ => _.HistoricCustomerMetrics)
                .ThenInclude(_ => _.Metric)
                .FirstAsync(_ => _.Id == customer.Id);

            return customer.HistoricCustomerMetrics.Any(_ => _.MetricId == metric.Id && _.Version == version.Value);
        }

        public async Task<IEnumerable<CustomerMetricWeeklyNumericAggregate>> GetAggregateMetricValuesNumeric(Metric metric)
        {
            // past twelve weeks worth of aggregation
            var twelveWeeksAgo = DateTime.Today.AddDays(-7 * 12);
            DateTimeOffset firstOfWeek = twelveWeeksAgo.FirstDayOfWeek(DayOfWeek.Monday);
            var numericAggregates = await context.CustomerMetricWeeklyNumericAggregates
            .Where(_ => _.MetricId == metric.Id && _.FirstOfWeek >= firstOfWeek)
            .OrderBy(_ => _.FirstOfWeek)
            .ToListAsync();

            return numericAggregates;
        }

        public async Task<IEnumerable<CustomerMetricWeeklyStringAggregate>> GetAggregateMetricValuesString(Metric metric)
        {
            // past twelve weeks worth of aggregation
            var twelveWeeksAgo = DateTime.Today.AddDays(-7 * 12);
            DateTimeOffset firstOfWeek = twelveWeeksAgo.FirstDayOfWeek(DayOfWeek.Monday);
            var stringAggregates = await context.CustomerMetricWeeklyStringAggregates
            .Where(_ => _.MetricId == metric.Id && _.FirstOfWeek >= firstOfWeek)
            .OrderBy(_ => _.FirstOfWeek)
            .ToListAsync();

            return stringAggregates;
        }
    }
}