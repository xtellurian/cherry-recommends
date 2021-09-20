using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal abstract class CommonEntityTypeConfigurationBase<T> : EnvironmentScopedEntityTypeConfigurationBase<T>, IEntityTypeConfiguration<T> where T : CommonEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder.HasIndex(_ => _.CommonId)
               .IsUnique();
            builder.Property(_ => _.Properties)
                .HasJsonConversion();
        }
    }
}