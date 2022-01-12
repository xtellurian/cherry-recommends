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
            builder.Property(_ => _.CommonId).IsRequired(true);
            builder.HasIndex(_ => _.CommonId);
            builder.HasIndex(_ => new { _.CommonId, _.EnvironmentId })
               .IsUnique()
               .HasFilter(null); // allow null values to participate in uniqueness
            builder.Property(_ => _.Properties)
                .HasJsonConversion();
        }
    }
}