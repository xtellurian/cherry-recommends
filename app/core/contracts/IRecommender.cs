using System.Threading.Tasks;

namespace SignalBox.Core
{
#nullable enable
    public interface IRecommender
    {
        ModelRegistration? ModelRegistration { get; }
    }
}