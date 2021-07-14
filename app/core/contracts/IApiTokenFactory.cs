using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IApiTokenFactory
    {
        Task<string> GetToken(string scope = null);
    }
}