using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IStorageContext
    {
        Task SaveChanges();
    }
}