using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class CustomerSegmentTypeConfiguration : IEntityTypeConfiguration<CustomerSegment>
    {
        public void Configure(EntityTypeBuilder<CustomerSegment> builder)
        {
            builder.HasKey(_ => new { _.CustomerId, _.SegmentId });

            builder
                .HasOne(left => left.Customer)
                .WithMany(right => right.Segments)
                .HasForeignKey(_ => _.CustomerId);

            builder
                .HasOne(left => left.Segment)
                .WithMany(right => right.InSegment)
                .HasForeignKey(_ => _.SegmentId);
        }
    }
}