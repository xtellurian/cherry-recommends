using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ICustomerEventStore
    {
        Task<CustomerEvent> Read(string eventId);
        Task<IEnumerable<CustomerEvent>> AddRange(IEnumerable<CustomerEvent> events);
        Task LoadMany<TProperty>(CustomerEvent entity, Expression<Func<CustomerEvent, IEnumerable<TProperty>>> propertyExpression) where TProperty : class;
        Task<IEnumerable<CustomerEvent>> Latest(DateTimeOffset after);
        Task<int> Count(Expression<Func<CustomerEvent, bool>> predicate = null);
        Task<TProperty> Max<TProperty>(Expression<Func<CustomerEvent, TProperty>> selector);
        Task<TProperty> Min<TProperty>(Expression<Func<CustomerEvent, TProperty>> selector);
        Task<TProperty> Min<TProperty>(Expression<Func<CustomerEvent, bool>> predicate, Expression<Func<CustomerEvent, TProperty>> selector);
        Task<int> CountTrackedUsers(Expression<Func<CustomerEvent, bool>> predicate = null);
        Task<IEnumerable<CustomerEvent>> ReadEventsForUser(Customer user, EventQueryOptions options = null, DateTimeOffset? since = null);
        Task<Paginated<CustomerEvent>> ReadEventsForUser(int page, Customer user, Expression<Func<CustomerEvent, bool>> predicate = null);
        Task<IEnumerable<CustomerEvent>> ReadEventsOfKind(string kind, DateTimeOffset? since = null, DateTimeOffset? until = null);
        Task<long> CountEventsOfKind(string kind, DateTimeOffset? since = null, DateTimeOffset? until = null);
        Task<IEnumerable<CustomerEvent>> ReadEventsOfType(string kind, string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null);
        Task<IEnumerable<CustomerEvent>> ReadEventsOfType(string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null);
        Task<IEnumerable<string>> ReadUniqueKinds();
        Task<IEnumerable<string>> ReadUniqueEventTypes();
        Task<IEnumerable<string>> ReadUniqueEventTypes(string kind);
        Task<long> CountEventsOfType(string kind, string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null);
        Task<long> CountEventsOfType(string eventType, DateTimeOffset? since = null, DateTimeOffset? until = null);
    }
}