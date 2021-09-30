using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public abstract class EFEnvironmentScopedEntityBase<T> : EFEntityStoreBase<T> where T : EnvironmentScopedEntity
    {
        private readonly IEnvironmentService environmentService;

        protected EFEnvironmentScopedEntityBase(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentService environmentService, Func<SignalBoxDbContext, DbSet<T>> selector)
        : base(contextProvider, selector)
        {
            this.environmentService = environmentService;
        }

        protected IEnvironmentStore environmentStore => new EFEnvironmentStore(contextProvider);
        protected override IQueryable<T> QuerySet => GetScopedQuery();

        public override async Task<T> Create(T entity)
        {
            if (this.IsEnvironmentScoped)
            {
                var environment = await environmentService.ReadCurrent(environmentStore);
                entity.Environment = environment;
                entity.EnvironmentId = environment?.Id;
            }

            return await base.Create(entity);
        }

        private IQueryable<T> GetScopedQuery()
        {
            return IsEnvironmentScoped
            ? base.QuerySet.Where(_ => _.EnvironmentId == environmentService.CurrentEnvironmentId)
            : base.QuerySet;
        }
    }
}