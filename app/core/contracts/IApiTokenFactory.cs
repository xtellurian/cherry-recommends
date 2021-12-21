using System.Threading.Tasks;
using SignalBox.Core.OAuth;

namespace SignalBox.Core
{
    public interface IApiTokenFactory
    {
        Task<TokenResponse> GetM2MToken(string scope = null);
        Task<TokenResponse> GetManagementToken(string scope = null);
    }
}