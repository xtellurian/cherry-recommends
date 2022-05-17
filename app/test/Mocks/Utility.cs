using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using SignalBox.Core;

namespace SignalBox.Test
{
    public static class Utility
    {
        public static Mock<ILogger<T>> MockLogger<T>()
        {
            return new Mock<ILogger<T>>();
        }
        public static Mock<ITelemetry> MockTelemetry()
        {
            var m = new Mock<ITelemetry>();
            m.Setup(_ => _.NewStopwatch(It.IsAny<bool>())).Returns(new System.Diagnostics.Stopwatch());
            return m;
        }
        public static Mock<IStorageContext> MockStorageContext()
        {
            return new Mock<IStorageContext>();
        }
        public static TestDateTimeProvider DateTimeProvider(DateTimeOffset? value = null)
        {
            return new TestDateTimeProvider(value ?? DateTimeOffset.Now);
        }

        private static readonly Random Rnd = new();
        public static TEntity WithId<TEntity>(this TEntity entity) where TEntity : Entity
        {
            entity.Id = Rnd.Next(1, 10000000);
            return entity;
        }
        public static TMock WithContext<TMock, TStore, TEntity>(this TMock mockStore, IStorageContext context = null)
        where TMock : Mock<TStore>
        where TStore : class, IEntityStore<TEntity>
        where TEntity : Entity
        {
            mockStore.Setup(_ => _.Context).Returns(context ?? MockStorageContext().Object);
            return mockStore;
        }

        public static void SetupStoreCreate<TMock, TStore, TEntity>(this TMock mockStore)
        where TMock : Mock<TStore>
        where TStore : class, IEntityStore<TEntity>
        where TEntity : Entity
        {
            mockStore.Setup(_ => _.Create(It.IsAny<TEntity>())).ReturnsAsync((TEntity entity) => entity);
        }
        public static void SetupStoreRead<TMock, TStore, TEntity>(this TMock mockStore, params TEntity[] entities)
        where TMock : Mock<TStore>
        where TStore : class, IEntityStore<TEntity>
        where TEntity : Entity
        {
            mockStore.Setup(_ => _.Read(It.IsAny<long>())).ReturnsAsync((long id) => entities.First(e => e.Id == id));
        }

        public static void SetupCommonStoreRead<TMock, TStore, TEntity>(this TMock mockStore, params TEntity[] entities)
        where TMock : Mock<TStore>
        where TStore : class, ICommonEntityStore<TEntity>
        where TEntity : CommonEntity
        {
            mockStore.Setup(_ => _.ReadFromCommonId(It.IsAny<string>())).ReturnsAsync((string id) => entities.First(e => e.CommonId == id));
            mockStore.Setup(_ => _.Read(It.IsAny<long>())).ReturnsAsync((long id) => entities.First(e => e.Id == id));
        }
    }
}