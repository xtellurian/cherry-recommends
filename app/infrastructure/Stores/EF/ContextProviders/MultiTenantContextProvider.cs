using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class MultiTenantContextProvider<T> : IDbContextProvider<T> where T : DbContext
    {
        public MultiTenantContextProvider(IDbContextFactory<T> contextFactory)
        {
            this.Context = contextFactory.CreateDbContext();
        }

        public T Context { get; private set; }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await this.Context.DisposeAsync();
            await DisposeAsyncCore();

            Dispose(disposing: false);
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
            GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Context?.Dispose();
            }

            this.Context = null;
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (this.Context is not null)
            {
                await this.Context.DisposeAsync().ConfigureAwait(false);
            }

            this.Context = null;
        }
    }
}