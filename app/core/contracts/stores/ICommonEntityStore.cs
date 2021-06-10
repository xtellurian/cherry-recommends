using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ICommonEntityStore<T> : IEntityStore<T> where T : CommonEntity
    {
        Task<T> ReadFromCommonId(string commonId);
        Task<bool> ExistsFromCommonId(string commonId);
        Task<string> FindCommonId(long id);
    }
}