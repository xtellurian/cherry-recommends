using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class PresentationOutcomeTypeConfiguration : EntityTypeConfigurationBase<PresentationOutcome>, IEntityTypeConfiguration<PresentationOutcome>
    {
        public override void Configure(EntityTypeBuilder<PresentationOutcome> builder)
        {
            base.Configure(builder);
        }
    }
}