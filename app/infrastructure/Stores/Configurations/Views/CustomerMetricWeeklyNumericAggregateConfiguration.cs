using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class CustomerMetricWeeklyNumericAggregateConfiguration : IEntityTypeConfiguration<CustomerMetricWeeklyNumericAggregate>
    {
        public void Configure(EntityTypeBuilder<CustomerMetricWeeklyNumericAggregate> builder)
        {
            builder.HasNoKey();
            builder.ToView("View_CustomerMetricWeeklyNumericAggregate");
        }
    }
}