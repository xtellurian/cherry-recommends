using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class CampaignArgumentTypeConfiguration : EntityTypeConfigurationBase<CampaignArgument>, IEntityTypeConfiguration<CampaignArgument>
    {
        public override void Configure(EntityTypeBuilder<CampaignArgument> builder)
        {
            base.Configure(builder);

            builder.Property(_ => _.ArgumentType).HasConversion<string>();

            builder
               .HasOne(_ => _.Campaign)
               .WithMany(_ => _.Arguments)
               .HasForeignKey(_ => _.CampaignId)
               .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasIndex(_ => new { _.CampaignId, _.CommonId })
                .IsUnique(); // ensure no duplicates
        }
    }
}