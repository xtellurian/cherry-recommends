using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    [Obsolete("This entity or table is obsolete.")]
    public class EFTrackedUserTouchpointStore : EFEntityStoreBase<TrackedUserTouchpoint>, ITrackedUserTouchpointStore
    {
        public EFTrackedUserTouchpointStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
         : base(contextProvider, c => c.TrackedUserTouchpoints)
        { }

        public async Task<int> CurrentMaximumTouchpointVersion(Customer customer, Touchpoint touchpoint)
        {
            // MaxAsync() w/ nullable int avoids 'Sequence contains no elements' error.
            return await context.Customers
               .Where(_ => _.Id == customer.Id)
               .SelectMany(_ => _.TrackedUserTouchpoints)
               .Where(_ => _.TouchpointId == touchpoint.Id)
               .MaxAsync(_ => (int?)_.Version) ?? 0;
        }

        public async Task<IEnumerable<Touchpoint>> GetTouchpointsFor(Customer customer)
        {
            return await context.Customers
                .Where(_ => _.Id == customer.Id)
                .SelectMany(_ => _.TrackedUserTouchpoints)
                .Select(_ => _.Touchpoint)
                .Distinct() // ensure we only return each touchpoint once.
                .ToListAsync();
        }

        public async Task<TrackedUserTouchpoint> ReadTouchpoint(Customer customer, Touchpoint touchpoint, int? version = null)
        {
            version ??= await CurrentMaximumTouchpointVersion(customer, touchpoint);
            customer = await context.Customers
                .Include(_ => _.TrackedUserTouchpoints)
                .ThenInclude(_ => _.Touchpoint)
                .FirstAsync(_ => _.Id == customer.Id);

            return customer.TrackedUserTouchpoints.First(_ => _.Touchpoint == touchpoint && _.Version == version.Value);

        }

        public async Task<bool> TouchpointExists(Customer customer, Touchpoint touchpoint, int? version = null)
        {
            version ??= await CurrentMaximumTouchpointVersion(customer, touchpoint);
            customer = await context.Customers
                .Include(_ => _.TrackedUserTouchpoints)
                .ThenInclude(_ => _.Touchpoint)
                .FirstAsync(_ => _.Id == customer.Id);

            return customer.TrackedUserTouchpoints.Any(_ => _.Touchpoint == touchpoint && _.Version == version.Value);
        }
    }
}