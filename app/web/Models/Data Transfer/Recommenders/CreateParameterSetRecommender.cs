using System.Collections.Generic;
using SignalBox.Core.Recommenders;

namespace SignalBox.Web.Dto
{
    public class CreateParameterSetRecommender : CreateRecommenderDtoBase
    {
        public IEnumerable<string> Parameters { get; set; }
        public IEnumerable<CreateOrUpdateRecommenderArgument> Arguments { get; set; }
        public IEnumerable<ParameterBounds> Bounds { get; set; }
    }
}