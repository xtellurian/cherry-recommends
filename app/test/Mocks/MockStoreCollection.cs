using System;
using System.Collections.Generic;
using Moq;
using SignalBox.Core;

namespace SignalBox.Test
{
    public class MockStoreCollection : IStoreCollection
    {
        private readonly Dictionary<Type, object> stores = new();
        public MockStoreCollection With<TStore, TEntity>(Mock<TStore> mock) where TStore : class, IStore<TEntity> where TEntity : class
        {
            stores.Add(typeof(TStore), mock);
            return this;
        }

        TStore IStoreCollection.ResolveStore<TStore, TEntity>()
        {
            var key = typeof(TStore);
            if (!stores.ContainsKey(key))
            {
                stores.Add(key, new Mock<TStore>());
            }

            var mock = stores[typeof(TStore)] as Mock<TStore>;
            return mock.Object;
        }
    }
}