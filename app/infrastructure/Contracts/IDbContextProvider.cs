using System;
using Microsoft.EntityFrameworkCore;

namespace SignalBox.Infrastructure
{
    public interface IDbContextProvider<T> : IDisposable, IAsyncDisposable where T : DbContext
    {
        T Context { get; }
    }
}