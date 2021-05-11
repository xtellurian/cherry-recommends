using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IEntityStore<T> : IStore<T> where T : Entity
    { }
}