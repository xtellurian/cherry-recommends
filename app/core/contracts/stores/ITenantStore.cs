using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ITenantStore
    {
        Task<IEnumerable<Tenant>> List();
        Task<Tenant> Read(long id);
        Task<Tenant> ReadFromName(string name);
        Task<Tenant> Create(Tenant tenant);
        Task Load<TProperty>(Tenant entity, Expression<Func<Tenant, TProperty>> propertyExpression) where TProperty : class;
        Task SaveChanges();
        Task<bool> TenantExists(string name);
    }
}