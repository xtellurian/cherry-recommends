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

        public async IAsyncEnumerable<Customer> Iterate()
        {
            bool hasMoreItems = await QuerySet.AnyAsync();
            if (!hasMoreItems)
            {
                yield break;
            }
            var maxId = await QuerySet.MaxAsync(_ => _.Id);
            var currentId = maxId + 1; // we query for ids less than this.
            while (hasMoreItems)
            {
                var results = await QuerySet
                    .Where(_ => _.Id < currentId)
                    .OrderByDescending(_ => _.Id)
                    .Take(PageSize)
                    .ToListAsync();

                if (results.Any())
                {
                    currentId = results.Min(_ => _.Id); // get the smallest in the result

                    foreach (var item in results)
                    {
                        yield return item;
                    }

                    hasMoreItems = await QuerySet.AnyAsync(_ => _.Id < currentId);
                }
                else
                {
                    // break out of the iteration. no more results.
                    hasMoreItems = false;
                }
            }
        }

        public async Task<IEnumerable<Customer>> CreateIfNotExists(IEnumerable<string> commonIds)
        {
            var users = new List<Customer>();
            foreach (var commonId in commonIds)
            {
                // Current behaviour is to not update the name.
                // this is because the name is auto-generated when recommenders are called
                // on a user that does not exist yet. 
                // Updating the name here would overwrite existing names that might be valid.
                users.Add(await this.CreateIfNotExists(commonId));
            }

            return users;
        }

        public async Task<Customer> CreateIfNotExists(string commonId, string name = null)
        {
            if (!await this.QuerySet.AnyAsync(_ => _.CommonId == commonId))
            {
                return await this.Create(new Customer(commonId, name));
            }
            else
            {
                return await this.ReadFromCommonId(commonId);
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