using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Shims
{
    public class ShimTenantStore : ITenantStore
    {
        public Task<Tenant> Create(Tenant tenant)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tenant>> List()
        {
            return Task.FromResult<IEnumerable<Tenant>>(new List<Tenant>());
        }

        public Task Load<TProperty>(Tenant entity, Expression<Func<Tenant, TProperty>> propertyExpression) where TProperty : class
        {
            throw new NotImplementedException();
        }

        public Task<Tenant> Read(long id)
        {
            throw new NotImplementedException();
        }

        public Task<Tenant> ReadFromName(string name)
        {
            throw new NotImplementedException();
        }

        public Task SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> TenantExists(string name)
        {
            return Task.FromResult(false);
        }
    }
}