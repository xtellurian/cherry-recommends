using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class RecommenderSegmentTypeConfiguration : IEntityTypeConfiguration<RecommenderSegment>
    {
        public void Configure(EntityTypeBuilder<RecommenderSegment> builder)
        {
            builder.HasKey(_ => new { _.RecommenderId, _.SegmentId });

            builder
                .HasOne(left => left.Recommender)
                .WithMany(right => right.RecommenderSegments)
                .HasForeignKey(_ => _.RecommenderId);

            builder
                .HasOne(left => left.Segment)
                .WithMany(right => right.RecommenderSegments)
                .HasForeignKey(_ => _.SegmentId);
        }
    }
}