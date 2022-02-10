using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class CustomerMetricDailyStringAggregateConfiguration : IEntityTypeConfiguration<CustomerMetricDailyStringAggregate>
    {
        public void Configure(EntityTypeBuilder<CustomerMetricDailyStringAggregate> builder)
        {
            builder.HasNoKey();
            builder.ToView("View_CustomerMetricDailyStringAggregate");
        }
    }
}