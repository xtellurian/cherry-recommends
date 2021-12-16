using SignalBox.Core;

namespace SignalBox.Web.Dto
{
    public class GetPersonalizedOfferDto : DtoBase
    {
        public Customer User { get; set; }
        // public TrackedUserProperties Properties { get; set; }
    }
}