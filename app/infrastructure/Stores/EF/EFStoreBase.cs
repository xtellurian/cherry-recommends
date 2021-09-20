using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public abstract class EFStoreBase<T> where T : class
    {
        protected readonly SignalBoxDbContext context;
        private readonly Func<SignalBoxDbContext, DbSet<T>> selector;
        protected DbSet<T> Set => selector(context);
        protected virtual IQueryable<T> QuerySet => selector(context).TagWith(EnvironmentScopingTag);

        protected EFStoreBase(SignalBoxDbContext context, Func<SignalBoxDbContext, DbSet<T>> selector)
        {
            this.context = context;
            this.selector = selector;
        }

        public IStorageContext Context => new EFStorageContext(context);
        protected virtual bool IsEnvironmentScoped => false;
        protected string EnvironmentScopingTag => IsEnvironmentScoped ? Tags.EnvironmentScoped : Tags.Unscoped;
    }
}
