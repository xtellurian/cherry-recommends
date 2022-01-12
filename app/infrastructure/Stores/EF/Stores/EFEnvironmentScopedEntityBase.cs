using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public abstract class EFEnvironmentScopedEntityBase<T> : EFEntityStoreBase<T> where T : EnvironmentScopedEntity
    {
        protected readonly IEnvironmentProvider environmentProvider;

        protected EFEnvironmentScopedEntityBase(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentProvider, Func<SignalBoxDbContext, DbSet<T>> selector)
        : base(contextProvider, selector)
        {
            this.environmentProvider = environmentProvider;
        }

        protected IEnvironmentStore EnvironmentStore => new EFEnvironmentStore(contextProvider);
        protected override IQueryable<T> QuerySet => GetScopedQuery();

        public override async Task<T> Create(T entity)
        {
            if (IsEnvironmentScoped && entity.EnvironmentId == null) // allow override environment ID
            {
                var environment = await environmentProvider.ReadCurrent(EnvironmentStore);
                entity.EnvironmentId ??= environment?.Id;
                entity.Environment ??= environment;
            }

            return await base.Create(entity);
        }

        private IQueryable<T> GetScopedQuery()
        {
            return IsEnvironmentScoped
            ? base.QuerySet.Where(_ => _.EnvironmentId == environmentProvider.CurrentEnvironmentId)
            : base.QuerySet;
        }
    }
}