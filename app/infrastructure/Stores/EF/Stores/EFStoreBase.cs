using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public abstract class EFStoreBase<T> where T : class
    {
        protected SignalBoxDbContext context => contextProvider.Context;
        protected readonly IDbContextProvider<SignalBoxDbContext> contextProvider;
        private readonly Func<SignalBoxDbContext, DbSet<T>> selector;
        protected DbSet<T> Set => selector(context);
        protected virtual IQueryable<T> QuerySet => selector(context).TagWith(EnvironmentScopingTag);

        protected EFStoreBase(IDbContextProvider<SignalBoxDbContext> contextProvider, Func<SignalBoxDbContext, DbSet<T>> selector)
        {
            this.contextProvider = contextProvider;
            this.selector = selector;
        }

        public IStorageContext Context => new EFStorageContext(contextProvider);
        protected virtual bool IsEnvironmentScoped => false;
        protected string EnvironmentScopingTag => IsEnvironmentScoped ? Tags.EnvironmentScoped : Tags.Unscoped;
    }
}
