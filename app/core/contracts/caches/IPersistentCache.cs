using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IPersistentCache<TCacheItem> where TCacheItem : class
    {
        Task Set(string key, TCacheItem value);
        Task<TCacheItem> Get(string key, System.Func<Task<TCacheItem>> resolver);
    }
}