using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#nullable enable
namespace SignalBox.Core
{
    public interface ICustomerStore : ICommonEntityStore<Customer>
    {
        Task<long> GetInternalId(string customerId);
        Task<IEnumerable<Customer>> CreateIfNotExists(IEnumerable<string> customerId);
        Task<Customer> CreateIfNotExists(string commonId, string? name = null);
        IAsyncEnumerable<Customer> Iterate();
    }
}