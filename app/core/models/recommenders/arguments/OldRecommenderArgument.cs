namespace SignalBox.Core.Campaigns
{
    public class OldRecommenderArgument
    {
        private DefaultArgumentContainer defaultValue;
        private object defaultArgumentValue;

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
        public object DefaultArgumentValue
        {
            get => defaultArgumentValue ?? this.DefaultValue?.Value; // try both
            set => defaultArgumentValue = value;
        }
        public bool IsRequired { get; set; } = false; // default not required.
    }

    public enum ArgumentTypes
    {
        Numerical,
        Categorical,
    }
}