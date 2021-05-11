using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class RuleTypeConfiguration: EntityTypeConfigurationBase<Rule>, IEntityTypeConfiguration<Rule>
    {
        public override void Configure(EntityTypeBuilder<Rule> builder)
        {
            base.Configure(builder);
        }
    }
}