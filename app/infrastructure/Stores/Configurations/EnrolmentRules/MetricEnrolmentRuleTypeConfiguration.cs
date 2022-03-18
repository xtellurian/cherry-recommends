using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;
using SignalBox.Core.Predicates;
using SignalBox.Core.Segments;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class MetricEnrolmentRuleTypeConfiguration : EnrolmentRuleTypeConfigurationBase<MetricEnrolmentRule>, IEntityTypeConfiguration<MetricEnrolmentRule>
    {
        public override void Configure(EntityTypeBuilder<MetricEnrolmentRule> builder)
        {
            base.Configure(builder);
            builder
                .HasOne(_ => _.Metric)
                .WithMany()
                .HasForeignKey(_ => _.MetricId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(_ => _.NumericPredicate).HasJsonConversion();
            builder.Property(_ => _.CategoricalPredicate).HasJsonConversion();

            builder.HasData(MetricEnrolmentRule.MoreThan10EventsEnrolmentRule);
        }
    }
}