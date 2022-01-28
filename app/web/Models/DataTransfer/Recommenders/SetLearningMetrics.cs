using System.Collections.Generic;
using System.Linq;
using SignalBox.Core;

#nullable enable
namespace SignalBox.Web.Dto
{
    public class SetLearningMetrics : DtoBase
    {
        public override void Validate()
        {
            base.Validate();
            if (MetricIds?.Count() > 2)
            {
                throw new BadRequestException("A maximum of 2 learning features are currently supported.");
            }
        }
        public bool? UseInternalId { get; set; }
        public IEnumerable<string>? FeatureIds
        {
            get => MetricIds; set
            {
                if (value != null)
                {
                    MetricIds = value;
                }
            }
        }

        public IEnumerable<string>? MetricIds { get; set; }
    }
}