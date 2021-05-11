namespace SignalBox.Core
{
    public class Iteration
    {
        public static Iteration First => new Iteration
        {
            Order = 1
        };

        public string Id { get; set; }
        public int Order { get; set; }
    }
}