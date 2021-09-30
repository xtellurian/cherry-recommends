using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IApiTokenFactory
    {
        Task<string> GetM2MToken(string scope = null);
        Task<string> GetManagementToken(string scope = null);
    }
}