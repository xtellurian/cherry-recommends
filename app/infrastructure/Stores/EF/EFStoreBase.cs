using System;
using Microsoft.EntityFrameworkCore;

namespace SignalBox.Infrastructure
{
    public abstract class EFStoreBase<T> where T : class
    {
        protected readonly SignalBoxDbContext context;
        private readonly Func<SignalBoxDbContext, DbSet<T>> selector;
        protected DbSet<T> Set => selector(context);

        protected EFStoreBase(SignalBoxDbContext context, Func<SignalBoxDbContext, DbSet<T>> selector)
        {
            this.context = context;
            this.selector = selector;
        }
    }
}
