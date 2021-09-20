using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal abstract class EnvironmentScopedEntityTypeConfigurationBase<T> : EntityTypeConfigurationBase<T>, IEntityTypeConfiguration<T> where T : EnvironmentScopedEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder.HasOne(_ => _.Environment)
                .WithMany()
                .HasForeignKey(_ => _.EnvironmentId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}