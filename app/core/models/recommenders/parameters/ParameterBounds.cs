using System.Collections.Generic;

namespace SignalBox.Core.Campaigns
{
#nullable enable
    public class ParameterBounds
    {
        public string CommonId { get; set; } = null!;
        public NumericalParameterBounds? NumericBounds { get; set; }
        public CategoricalParameterBounds? CategoricalBounds { get; set; }

    }

    public class NumericalParameterBounds
    {
        public double Min { get; set; }
        public double Max { get; set; } = double.MaxValue;
    }

    public class CategoricalParameterBounds
    {
        public List<string> Categories { get; set; } = new List<string>();
    }
}