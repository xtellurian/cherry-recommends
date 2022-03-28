using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class ShopifyCode
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string ShopifyUrl { get; set; }
    }
}