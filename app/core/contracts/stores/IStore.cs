using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IStore<T> where T : class
    {
        Task<bool> Exists(long id);
        Task<T> Create(T entity);
        Task<T> Read(long id);
        Task<T> Update(T entity);
        Task<IEnumerable<T>> List();
        Task<bool> Remove(long id);
    }
}