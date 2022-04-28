using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IIntegratedSystemCredentialStore : IEntityStore<IntegratedSystemCredential>
    {
        Task<EntityResult<IntegratedSystemCredential>> ReadFromKey(string key);
    }
}