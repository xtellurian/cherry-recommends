using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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

        public async Task<IEnumerable<CustomerMetricWeeklyNumericAggregate>> GetAggregateMetricValuesNumeric(Metric metric, int weeksAgo = 11)
        {
            if (metric == null)
            {
                throw new ArgumentNullException(nameof(metric));
            }
            // past twelve weeks worth of aggregation
            var WeeksAgoDt = DateTime.Today.AddDays(-7 * weeksAgo);
            DateTimeOffset firstOfWeek = WeeksAgoDt.FirstDayOfWeek(DayOfWeek.Monday);
            var numericAggregates = await context.CustomerMetricWeeklyNumericAggregates
                .Where(_ => _.MetricId == metric.Id && _.FirstOfWeek >= firstOfWeek)
                .OrderBy(_ => _.FirstOfWeek)
                .ToListAsync();

            return numericAggregates;
        }

        public async Task<IEnumerable<CustomerMetricWeeklyStringAggregate>> GetAggregateMetricValuesString(Metric metric, int weeksAgo = 11)
        {
            if (metric == null)
            {
                throw new ArgumentNullException(nameof(metric));
            }
            // past twelve weeks worth of aggregation
            var WeeksAgoDt = DateTime.Today.AddDays(-7 * weeksAgo);
            DateTimeOffset firstOfWeek = WeeksAgoDt.FirstDayOfWeek(DayOfWeek.Monday);
            var stringAggregates = await context.CustomerMetricWeeklyStringAggregates
                .Where(_ => _.MetricId == metric.Id && _.FirstOfWeek >= firstOfWeek)
                .OrderBy(_ => _.FirstOfWeek)
                .ToListAsync();

            return stringAggregates;
        }

        public async Task<IEnumerable<MetricDailyBinValueNumeric>> GetMetricBinValuesNumeric(Metric metric, int binCount = 12)
        {
            var numericBinValues = await context.MetricDailyBinNumericValues
                .FromSqlInterpolated($"dbo.sp_NumericMetricBinning {metric.Id},{binCount}")
                .ToListAsync();
            return numericBinValues;
        }

        public async Task<IEnumerable<MetricDailyBinValueString>> GetMetricBinValuesString(Metric metric, int topKValue = 12)
        {
            var categoricalBinValues = await context.MetricDailyBinStringValues
                .FromSqlInterpolated($"dbo.sp_CategoricalMetricBinning {metric.Id},{topKValue}")
                .ToListAsync();
            return categoricalBinValues;
        }

        public async Task<IEnumerable<MetricCustomerExport>> GetMetricCustomerExports(Metric metric)
        {
            var result = await context.LatestFeatureVersions
                .Join(context.Customers, _ => _.CustomerId, _ => _.Id, (latest, customer) => new
                {
                    latest,
                    customer
                })
                .Join(context.Metrics, _ => _.latest.MetricId, _ => _.Id, (g, metric) => new
                {
                    latest = g.latest,
                    customer = g.customer,
                    metric = metric
                })
                .Join(context.HistoricCustomerMetrics, _ => _.latest.HistoricCustomerMetricId, _ => _.Id, (g, historic) => new
                {
                    latest = g.latest,
                    customer = g.customer,
                    metric = g.metric,
                    historic = historic
                })
                .Where(_ => _.metric.Id == metric.Id)
                .Select(_ => new MetricCustomerExport
                {
                    CustomerId = _.customer.CommonId,
                    Email = "",
                    MetricName = _.metric.Name,
                    MetricValue = _.historic.NumericValue.HasValue ? _.historic.NumericValue.Value : _.historic.StringValue
                }).ToListAsync();

            return result;
        }
    }
}