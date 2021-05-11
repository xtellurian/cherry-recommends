using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    internal class OfferPresentationTypeConfiguration : EntityTypeConfigurationBase<OfferRecommendation>, IEntityTypeConfiguration<OfferRecommendation>
    {
        public override void Configure(EntityTypeBuilder<OfferRecommendation> builder)
        {
            base.Configure(builder);

            builder
                .Property(_ => _.Features)
                .HasConversion(
                v => JsonSerializer.Serialize(v, null),
                v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null),
                new ValueComparer<IDictionary<string, object>>(
                    (c1, c2) => c1.Keys.SequenceEqual(c2.Keys),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => (IDictionary<string, object>)c.ToList()));
        }
    }
}