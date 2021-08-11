using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class RewardSelectorTypeConfiguration : EntityTypeConfigurationBase<RewardSelector>, IEntityTypeConfiguration<RewardSelector>
    {
        public override void Configure(EntityTypeBuilder<RewardSelector> builder)
        {
            base.Configure(builder);
            builder.Property(_ => _.SelectorType).HasConversion<string>();
            builder
                .HasIndex(_ => new { actionName = _.ActionName, selectorType = _.SelectorType })
                .IsUnique();
        }
    }
}