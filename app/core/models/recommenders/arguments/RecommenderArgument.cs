namespace SignalBox.Core.Recommenders
{
    public class RecommenderArgument
    {
        private DefaultArgumentContainer defaultValue;

        public string CommonId { get; set; } // use commonId for external API consistency
        public ArgumentTypes ArgumentType { get; set; }
        public DefaultArgumentContainer DefaultValue
        {
            get { return defaultValue; }
            set
            {
                DefaultArgumentValue = value?.Value;
                defaultValue = value;
            }
        }
        public object DefaultArgumentValue { get; set; }
        public bool IsRequired { get; set; } = false; // default not required.
    }

    public enum ArgumentTypes
    {
        Numerical,
        Categorical,
    }
}