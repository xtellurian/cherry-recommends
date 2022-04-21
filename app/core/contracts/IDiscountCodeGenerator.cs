using System.Threading.Tasks;

namespace SignalBox.Core
{
#nullable enable
    public interface IDiscountCodeGenerator
    {
        IntegratedSystemTypes SystemType { get; }
        Task Generate(IntegratedSystem system, RecommendableItem promotion, DiscountCode discountCode);
    }
}