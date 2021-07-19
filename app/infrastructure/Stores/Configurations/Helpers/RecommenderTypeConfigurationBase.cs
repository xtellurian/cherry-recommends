using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal abstract class RecommenderTypeConfigurationBase<T>
       : EntityTypeConfigurationBase<T>, IEntityTypeConfiguration<T> where T : RecommenderEntityBase
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder
                .HasMany(_ => _.TargetVariableValues)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasMany(_ => _.RecommenderInvokationLogs)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}