using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFCustomerEventStore : EFEnvironmentScopedEntityStoreBase<CustomerEvent>, ICustomerEventStore
    {
        protected override Expression<Func<CustomerEvent, DateTimeOffset>> defaultOrderBy => _ => _.Timestamp;
        public EFCustomerEventStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentProvider)
        : base(contextProvider, environmentProvider, (c) => c.CustomerEvents)
        { }

        protected override bool IsEnvironmentScoped => true;

        public async Task<CustomerEvent> Read(string eventId)
        {
            if (await QuerySet.AnyAsync(_ => _.EventId == eventId))
            {
                return await QuerySet.FirstAsync(_ => _.EventId == eventId);
            }
            else if (long.TryParse(eventId, out var id))
            {
                try
                {
                    return await QuerySet.SingleAsync(_ => _.Id == id);
                }
                catch (Exception ex)
                {
                    throw new StorageException($"Event Id {eventId} not found", ex);
                }
            }
            else
            {
                throw new StorageException($"Event Id {eventId} not found");
            }
        }
        public async Task<IEnumerable<CustomerEvent>> AddRange(IEnumerable<CustomerEvent> events)
        {
            foreach (var e in events)
            {
                e.EnvironmentId = environmentProvider.CurrentEnvironmentId;
            }

            // no need to check if event already exists, just let the db fail if already existing
            await Set.AddRangeAsync(events);

            return events;
        }

        public async Task<int> CountTrackedUsers(Expression<Func<CustomerEvent, bool>> predicate = null)
        {
            return await QuerySet
                .Where(predicate ?? ((x) => true))
                .Select(_ => _.CustomerId)
                .Distinct()
                .CountAsync();
        }

        public async Task<IEnumerable<CustomerEvent>> ReadEventsForUser(Customer customer, EventQueryOptions options = null, DateTimeOffset? since = null)
        {
            options ??= new EventQueryOptions();
            options.Filter ??= _ => true;
            since ??= DateTimeOffset.MinValue;

            var query = QuerySet
                .Where(_ => _.TrackedUserId == customer.Id)
                .Where(options.Filter)
                .Where(_ => _.Timestamp > since);

            if (options.NoTracking)
            {
                query = query.AsNoTracking();
            }

            var results = await query
                .OrderByDescending(_ => _.Timestamp)
                .ToListAsync();

            return results;
        }

        public async Task<Paginated<CustomerEvent>> ReadEventsForUser(IPaginate paginate,
                                                                      Customer customer,
                                                                      Expression<Func<CustomerEvent, bool>> predicate = null)
        {
            predicate ??= _ => true; // default to all
            var pageSize = paginate.PageSize ?? DefaultPageSize;
            Expression<Func<CustomerEvent, bool>> selectCustomer = _ => _.CustomerId == customer.CustomerId;
            var itemCount = await QuerySet.Where(selectCustomer).CountAsync(predicate);
            List<CustomerEvent> results;

            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await QuerySet
                    .Where(predicate)
                    .Where(selectCustomer)
                    .OrderByDescending(_ => _.Id)
                    .Skip((paginate.SafePage - 1) * pageSize).Take(pageSize)
                    .OrderByDescending(_ => _.Timestamp)
                    .ToListAsync();
            }
            else
            {
                results = new List<CustomerEvent>();
            }
            var pageCount = (int)Math.Ceiling((double)itemCount / pageSize);
            return new Paginated<CustomerEvent>(results, pageCount, itemCount, paginate.SafePage);
        }

        public async Task<IEnumerable<CustomerEvent>> ReadEventsOfType(string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            since ??= DateTimeOffset.MinValue;
            until ??= DateTimeOffset.MaxValue;
            return await QuerySet.Where(_ => _.EventType == eventType && _.Timestamp > since && _.Timestamp < until).ToListAsync();
        }

        public async Task<IEnumerable<CustomerEvent>> ReadEventsOfType(EventKinds kind, string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            since ??= DateTimeOffset.MinValue;
            until ??= DateTimeOffset.MaxValue;
            return await QuerySet.Where(_ => _.EventKind == kind && _.EventType == eventType && _.Timestamp > since && _.Timestamp < until).ToListAsync();
        }

        public async Task<IEnumerable<CustomerEvent>> ReadEventsOfKind(EventKinds kind, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            since ??= DateTimeOffset.MinValue;
            until ??= DateTimeOffset.MaxValue;
            return await QuerySet.Where(_ => _.EventKind == kind && _.Timestamp > since && _.Timestamp < until).ToListAsync();
        }

        public async Task<IEnumerable<string>> ReadUniqueEventTypes(EventKinds kind)
        {
            return await QuerySet.Where(_ => _.EventKind == kind).Select(_ => _.EventType).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<string>> ReadUniqueEventTypes()
        {
            return await QuerySet.Select(_ => _.EventType).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<string>> ReadUniqueKinds()
        {
            return await QuerySet.Select(_ => _.Kind).Distinct().ToListAsync();
        }

        public async Task<long> CountEventsOfKind(EventKinds kind, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            since ??= DateTimeOffset.MinValue;
            until ??= DateTimeOffset.MaxValue;
            Expression<Func<CustomerEvent, bool>> predicate = _ => _.EventKind == kind && _.Timestamp > since && _.Timestamp < until;
            return await base.Count(predicate);
        }
        public async Task<long> CountEventsOfType(EventKinds kind, string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            since ??= DateTimeOffset.MinValue;
            until ??= DateTimeOffset.MaxValue;
            Expression<Func<CustomerEvent, bool>> predicate = _ => _.EventKind == kind && _.EventType == eventType && _.Timestamp > since && _.Timestamp < until;
            return await base.Count(predicate);
        }
        public async Task<long> CountEventsOfType(string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null)
        {
            since ??= DateTimeOffset.MinValue;
            until ??= DateTimeOffset.MaxValue;
            Expression<Func<CustomerEvent, bool>> predicate = _ => _.EventType == eventType && _.Timestamp > since && _.Timestamp < until;
            return await base.Count(predicate);
        }

        public async Task<IEnumerable<CustomerEvent>> Latest(DateTimeOffset after)
        {
            return await QuerySet
                .Where(_ => _.Timestamp > after) // filtering by this column, has an index, and therefore is performant.
                .OrderByDescending(_ => _.Id) // this should be more performant than OrderBy(Created), and be mostly the same
                .Take(32).Skip(0)
                .ToListAsync();
        }

        public async Task<Paginated<CustomerEvent>> ReadEventsForBusiness(IPaginate paginate, Business business)
        {
            var pageSize = paginate.PageSize ?? DefaultPageSize;
            var result = await context.CustomerEvents
                .Join(context.Customers, evt => evt.TrackedUserId, cust => cust.Id, (customerEvent, customer) => new
                {
                    customerEvent,
                    customer
                })
                .Join(context.Businesses, combined => combined.customer.BusinessMembership.BusinessId, biz => biz.Id, (x, business) => new
                {
                    customerEvent = x.customerEvent,
                    customer = x.customer,
                    business = business
                })
                .Where(_ => _.business.Id == business.Id)
                .Select(_ => _.customerEvent)
                .OrderByDescending(_ => _.Timestamp)
                .Skip((paginate.SafePage - 1) * pageSize).Take(pageSize)
                .ToListAsync();

            int itemCount = result.Count;
            var pageCount = (int)Math.Ceiling((double)itemCount / pageSize);
            return new Paginated<CustomerEvent>(result, pageCount, itemCount, paginate.SafePage);
        }

        public async Task<IEnumerable<CustomerEvent>> ReadEventsForBusiness(Business business, EventQueryOptions options = null, DateTimeOffset? since = null)
        {
            options ??= new EventQueryOptions();
            options.Filter ??= _ => true;
            since ??= DateTimeOffset.MinValue;

            var query = context.CustomerEvents
                .Join(context.Customers, evt => evt.TrackedUserId, cust => cust.Id, (customerEvent, customer) => new
                {
                    customerEvent,
                    customer
                })
                .Join(context.Businesses, combined => combined.customer.BusinessMembership.BusinessId, biz => biz.Id, (x, business) => new
                {
                    customerEvent = x.customerEvent,
                    customer = x.customer,
                    business = business
                })
                .Where(_ => _.business.Id == business.Id)
                .Where(_ => _.customerEvent.Timestamp > since)
                .Select(_ => _.customerEvent)
                .Where(options.Filter);

            if (options.NoTracking)
            {
                query = query.AsNoTracking();
            }

            var results = await query
                .OrderByDescending(_ => _.Timestamp)
                .ToListAsync();

            return results;
        }
    }
}