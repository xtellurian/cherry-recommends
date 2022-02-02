using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#nullable enable
namespace SignalBox.Core
{
    public interface ICustomerStore : ICommonEntityStore<Customer>
    {
        Task<long> GetInternalId(string customerId);
        Task<IEnumerable<Customer>> CreateIfNotExists(IEnumerable<PendingCustomer> pendingCustomers);
        Task<Customer> CreateIfNotExists(PendingCustomer pending);
    }
}