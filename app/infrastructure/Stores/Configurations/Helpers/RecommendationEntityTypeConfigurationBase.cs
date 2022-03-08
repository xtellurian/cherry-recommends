using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommendations;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal abstract class RecommendationEntityTypeConfigurationBase<T>
        : EnvironmentScopedEntityTypeConfigurationBase<T>, IEntityTypeConfiguration<T> where T : RecommendationEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder.Property(_ => _.RecommenderType).HasConversion<string>();
            builder.Ignore(_ => _.IsFromCache);

            builder.Ignore(_ => _.TrackedUser);
            builder
                .HasOne(_ => _.Customer)
                .WithMany()
                .HasForeignKey(_ => _.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(_ => _.Business)
                .WithMany()
                .HasForeignKey(_ => _.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}