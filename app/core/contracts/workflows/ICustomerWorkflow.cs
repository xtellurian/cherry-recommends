using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ICustomerWorkflow
    {
        Task<Customer> CreateOrUpdate(PendingCustomer pendingCustomer, bool saveOnComplete = true);
        Task<IEnumerable<Customer>> CreateOrUpdate(IEnumerable<PendingCustomer> pendingCustomers, bool saveOnComplete = true);
    }
}