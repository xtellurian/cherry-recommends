using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class CustomerMetricWeeklyStringAggregateConfiguration : IEntityTypeConfiguration<CustomerMetricWeeklyStringAggregate>
    {
        public void Configure(EntityTypeBuilder<CustomerMetricWeeklyStringAggregate> builder)
        {
            builder.HasNoKey();
            builder.ToView("View_CustomerMetricWeeklyStringAggregate");
        }
    }
}