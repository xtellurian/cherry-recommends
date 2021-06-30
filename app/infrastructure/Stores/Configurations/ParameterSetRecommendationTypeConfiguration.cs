using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommendations;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class ParameterSetRecommendationTypeConfiguration : RecommendationEntityTypeConfigurationBase<ParameterSetRecommendation>, IEntityTypeConfiguration<ParameterSetRecommendation>
    {
        public override void Configure(EntityTypeBuilder<ParameterSetRecommendation> builder)
        {
            base.Configure(builder);
        }
    }
}