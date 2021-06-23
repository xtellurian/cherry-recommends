namespace SignalBox.Core.Recommenders
{
    public class RecommenderArgument
    {
        public string CommonId { get; set; } // use commonId for external API consistency
        public ArgumentTypes ArgumentType { get; set; }
        public DefaultArgumentValue DefaultValue { get; set; }
        public bool IsRequired { get; set; } = false; // default not required.
    }

    public enum ArgumentTypes
    {
        Numerical,
        Categorical,
    }
}