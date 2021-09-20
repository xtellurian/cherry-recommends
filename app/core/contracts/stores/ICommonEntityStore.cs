using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ICommonEntityStore<T> : IEntityStore<T> where T : CommonEntity
    {
        Task<T> ReadFromCommonId(string commonId);
        Task<bool> ExistsFromCommonId(string commonId);
        Task<T> ReadFromCommonId<TProperty>(string commonId, System.Linq.Expressions.Expression<System.Func<T, TProperty>> include);
        Task<Paginated<T>> Query(int page, string searchTerm);
    }
}