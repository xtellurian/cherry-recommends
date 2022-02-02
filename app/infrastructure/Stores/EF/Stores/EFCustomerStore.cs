using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFCustomerStore : EFCommonEntityStoreBase<Customer>, ICustomerStore
    {
        protected override bool IsEnvironmentScoped => true;
        public EFCustomerStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentProvider)
        : base(contextProvider, environmentProvider, (c) => c.Customers)
        { }

        public async Task<IEnumerable<Customer>> CreateIfNotExists(IEnumerable<PendingCustomer> pendingCustomers)
        {
            var users = new List<Customer>();
            foreach (var pending in pendingCustomers)
            {
                // Current behaviour is to not update the name.
                // this is because the name is auto-generated when recommenders are called
                // on a user that does not exist yet. 
                // Updating the name here would overwrite existing names that might be valid.
                users.Add(await this.CreateIfNotExists(pending));
            }

            return users;
        }

        public async Task<Customer> CreateIfNotExists(PendingCustomer pendingCustomer)
        {
            environmentProvider.SetOverride(pendingCustomer.EnvironmentId);
            if (!await this.QuerySet.AnyAsync(_ => _.CommonId == pendingCustomer.CommonId))
            {
                return await this.Create(new Customer(pendingCustomer.CommonId, pendingCustomer.Name));
            }
            else
            {
                return await this.ReadFromCommonId(pendingCustomer.CommonId);
            }
        }

        public async Task<long> GetInternalId(string commonId)
        {
            var entity = await QuerySet.SingleAsync(_ => _.CommonId == commonId);
            return entity.Id;
        }

        public async Task<Customer> ReadFromCommonUserId(string commonId)
        {
            return await QuerySet.SingleAsync(_ => _.CommonId == commonId);
        }
    }
}