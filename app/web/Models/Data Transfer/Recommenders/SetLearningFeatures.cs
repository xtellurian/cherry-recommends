using System.Collections.Generic;
using System.Linq;
using SignalBox.Core;

#nullable enable
namespace SignalBox.Web.Dto
{
    public class SetLearningFeatures : DtoBase
    {
        public override void Validate()
        {
            base.Validate();
            if (FeatureIds?.Count() > 2)
            {
                throw new BadRequestException("A maximum of 2 learning features are currently supported.");
            }
        }
        public bool? UseInternalId { get; set; }
        public IEnumerable<string>? FeatureIds { get; set; }
    }
}