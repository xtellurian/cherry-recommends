using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class IntegratedSystemTypeConfiguration : EntityTypeConfigurationBase<IntegratedSystem>, IEntityTypeConfiguration<IntegratedSystem>
    {
        public override void Configure(EntityTypeBuilder<IntegratedSystem> builder)
        {
            base.Configure(builder);
            builder.Property(t => t.Name)
                .IsRequired();
            builder.Property(t => t.SystemType)
                .IsRequired();
        }
    }
}