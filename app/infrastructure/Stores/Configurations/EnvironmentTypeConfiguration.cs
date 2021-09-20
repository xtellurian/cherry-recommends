using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class EnvironmentTypeConfiguration : EntityTypeConfigurationBase<Core.Environment>, IEntityTypeConfiguration<Core.Environment>
    {
        public override void Configure(EntityTypeBuilder<Core.Environment> builder)
        {
            builder
                .Property(_ => _.Name)
                .HasMaxLength(32)
                .IsRequired();

            base.Configure(builder);
        }
    }
}