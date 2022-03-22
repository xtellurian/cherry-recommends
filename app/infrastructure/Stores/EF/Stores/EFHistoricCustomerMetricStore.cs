using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
#nullable enable
    public class EFHistoricCustomerMetricStore : EFEntityStoreBase<HistoricCustomerMetric>, IHistoricCustomerMetricStore
    {
        public EFHistoricCustomerMetricStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, c => c.HistoricCustomerMetrics)
        { }

        public async Task<int> CurrentMaximumCustomerMetricVersion(Customer customer, Metric metric)
        {
            var latest = await LatestCustomerMetricValue(customer, metric);
            return latest?.MaxVersion ?? 0;
        }

        public async Task<LatestMetric?> LatestCustomerMetricValue(Customer customer, Metric metric)
        {
            var latest = await context.LatestMetrics
                .Where(_ => _.MetricId == metric.Id && _.CustomerId == customer.Id)
                .FirstOrDefaultAsync();
            return latest;
        }

        public async IAsyncEnumerable<LatestMetric> IterateLatest(long metricId, Expression<Func<LatestMetric, bool>>? predicate = null)
        {
            var localSet = context.LatestMetrics; // set for this query
            predicate ??= _ => true;
            bool hasMoreItems = await localSet.AnyAsync(predicate);
            if (!hasMoreItems)
            {
                yield break;
            }
            var maxId = await localSet.MaxAsync(_ => _.HistoricCustomerMetricId);
            var currentId = maxId + 1; // we query for ids less than this.
            while (hasMoreItems)
            {
                List<LatestMetric> results = await localSet
                        .Where(_ => _.CustomerId != null) // only get values where the CustomerID is set. Ignore Businesses and Global.
                        .Where(_ => _.MetricId == metricId)
                        .Where(predicate)
                        .Where(_ => _.HistoricCustomerMetricId < currentId)
                        .OrderByDescending(_ => _.HistoricCustomerMetricId) // descending here
                        .Take(DefaultPageSize) // use the default page size during iteration
                        .ToListAsync();

                if (results.Any())
                {
                    currentId = results.Min(_ => _.HistoricCustomerMetricId); // get the smallest in the result

                    foreach (var item in results)
                    {
                        yield return item;
                    }

                    hasMoreItems = await localSet.AnyAsync(_ => _.HistoricCustomerMetricId < currentId);
                }
                else
                {
                    // break out of the iteration. no more results.
                    hasMoreItems = false;
                }
            }
        }

        public async Task<IEnumerable<Metric>> GetMetricsFor(Customer customer)
        {
            var features = await context.HistoricCustomerMetrics
                .Where(_ => _.CustomerId == customer.Id)
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

        public async Task<IEnumerable<MetricDailyBinValueNumeric>> GetMetricBinValuesNumeric(Metric metric, int? binCount = 12)
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
            var result = await context.LatestMetrics
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