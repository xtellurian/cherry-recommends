namespace SignalBox.Core
{
    public class Score
    {
        public static Score DefaultScore => new Score
        {
            Value = 0
        };

        public double Value { get; set; }
    }
}