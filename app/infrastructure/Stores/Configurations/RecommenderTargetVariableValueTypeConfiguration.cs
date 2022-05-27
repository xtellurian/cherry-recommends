using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Campaigns;

namespace SignalBox.Infrastructure.EntityFramework
{
    [Obsolete("This entity or table is obsolete.")]
    internal class RecommenderTargetVariableValueTypeConfiguration
    : EntityTypeConfigurationBase<RecommenderTargetVariableValue>, IEntityTypeConfiguration<RecommenderTargetVariableValue>
    {
        public override void Configure(EntityTypeBuilder<RecommenderTargetVariableValue> builder)
        {
            base.Configure(builder);
            // ensure that the name version and recommender combo is unique
            builder
                .HasIndex(_ => new { _.RecommenderId, _.Name, _.Version })
                .IsUnique();

        }
    }
}