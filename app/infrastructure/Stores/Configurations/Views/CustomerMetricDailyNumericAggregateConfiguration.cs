using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class CustomerMetricDailyNumericAggregateConfiguration : IEntityTypeConfiguration<CustomerMetricDailyNumericAggregate>
    {
        public void Configure(EntityTypeBuilder<CustomerMetricDailyNumericAggregate> builder)
        {
            builder.HasNoKey();
            builder.ToView("View_CustomerMetricDailyNumericAggregate");
        }
    }
}