namespace SignalBox.Core
{
    public class OfferScore
    {
        public static OfferScore DefaultScore => new OfferScore
        {
            Value = 0
        };

        public double Value { get; set; }
    }
}