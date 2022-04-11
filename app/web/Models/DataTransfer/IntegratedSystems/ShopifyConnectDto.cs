using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    /// <summary> Used to created the integrated system with type Shopify </summary>
    public class ShopifyConnectDto
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Shop { get; set; }
        /// <summary> Default environment is null </summary>
        public long? EnvironmentId { get; set; }
    }
}