using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ICustomerEventStore : IEntityStore<CustomerEvent>
    {
        Task<CustomerEvent> Read(string eventId);
        /// <summary>
        /// Deletes all events for a customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>Number of events deleted</returns>
        Task<int> RemoveForCustomer(Customer customer);
        Task<IEnumerable<CustomerEvent>> AddRange(IEnumerable<CustomerEvent> events);
        Task<IEnumerable<CustomerEvent>> Latest(DateTimeOffset after);
        Task<Paginated<CustomerEvent>> Latest(IPaginate paginate);
        Task<TProperty> Max<TProperty>(Expression<Func<CustomerEvent, TProperty>> selector);
        Task<TProperty> Min<TProperty>(Expression<Func<CustomerEvent, TProperty>> selector);
        Task<TProperty> Min<TProperty>(Expression<Func<CustomerEvent, bool>> predicate, Expression<Func<CustomerEvent, TProperty>> selector);
        Task<int> CountTrackedUsers(Expression<Func<CustomerEvent, bool>> predicate = null);
        Task<IEnumerable<CustomerEvent>> ReadEventsForUser(Customer user, EventQueryOptions options = null, DateTimeOffset? since = null);
        Task<Paginated<CustomerEvent>> ReadEventsForUser(IPaginate paginate, Customer user, Expression<Func<CustomerEvent, bool>> predicate = null);
        Task<IEnumerable<CustomerEvent>> ReadEventsOfKind(EventKinds kind, DateTimeOffset? since = null, DateTimeOffset? until = null);
        Task<long> CountEventsOfKind(EventKinds kind, DateTimeOffset? since = null, DateTimeOffset? until = null);
        Task<IEnumerable<CustomerEvent>> ReadEventsOfType(EventKinds kind, string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null);
        Task<IEnumerable<CustomerEvent>> ReadEventsOfType(string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null);
        Task<IEnumerable<string>> ReadUniqueEventTypes();
        Task<IEnumerable<string>> ReadUniqueEventTypes(EventKinds kind);
        Task<long> CountEventsOfType(EventKinds kind, string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null);
        Task<long> CountEventsOfType(string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null);
        Task<Paginated<CustomerEvent>> ReadEventsForBusiness(IPaginate paginate, Business business);
        Task<IEnumerable<CustomerEvent>> ReadEventsForBusiness(Business business, EventQueryOptions options = null, DateTimeOffset? since = null);
    }
}