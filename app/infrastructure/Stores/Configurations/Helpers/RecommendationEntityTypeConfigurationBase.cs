using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommendations;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal abstract class RecommendationEntityTypeConfigurationBase<T>
        : EntityTypeConfigurationBase<T>, IEntityTypeConfiguration<T> where T : RecommendationEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder.Property(_ => _.RecommenderType).HasConversion<string>();
            builder.Ignore(_ => _.IsFromCache);
        }
    }
}