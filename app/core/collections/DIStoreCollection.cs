using System;

namespace SignalBox.Core
{
#nullable enable
    public class DIStoreCollection : IStoreCollection
    {
        private readonly IServiceProvider provider;

        public DIStoreCollection(IServiceProvider provider)
        {
            this.provider = provider;
        }
        public TStore ResolveStore<TStore, TEntity>() where TStore : class, IStore<TEntity> where TEntity : class
        {
            return provider.GetService(typeof(TStore)) as TStore ?? throw new System.ArgumentException($"No registered store for type {typeof(TStore)}"); ;
        }
    }
}
