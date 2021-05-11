using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class ExperimentTypeConfiguration : EntityTypeConfigurationBase<Experiment>, IEntityTypeConfiguration<Experiment>
    {
        public override void Configure(EntityTypeBuilder<Experiment> builder)
        {
            base.Configure(builder);
            builder.OwnsMany(e => e.Iterations,
                n => n.Property(_ => _.Id)
                    .ValueGeneratedOnAdd());
        }
    }
}