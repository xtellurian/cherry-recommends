using SignalBox.Core;

namespace SignalBox.Web.Dto
{
#nullable enable
    public class BaselinePromotionDto
    {
        public string? ItemId { get; set; } // backwards compat only
        public string? PromotionId { get; set; }

        public string GetPromotionId()
        {
            var id = PromotionId ?? ItemId;
            if (string.IsNullOrEmpty(id))
            {
                throw new BadRequestException("PromotionId is required");
            }
            return id;
        }
    }
}