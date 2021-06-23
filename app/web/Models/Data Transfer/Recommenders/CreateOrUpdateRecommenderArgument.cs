using SignalBox.Core.Recommenders;

namespace SignalBox.Web.Dto
{
    public class CreateOrUpdateRecommenderArgument
    {
        public string CommonId { get; set; } // use commonId for external API consistency
        public ArgumentTypes ArgumentType { get; set; }
        public string DefaultValue { get; set; }
        public bool IsRequired { get; set; } = false; // default not required.


    }
}