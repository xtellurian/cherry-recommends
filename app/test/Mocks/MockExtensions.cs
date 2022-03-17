using Moq;
using SignalBox.Core;

namespace SignalBox.Test
{
    public static class MockExtensions
    {
        public static void SetupContext<T, TEntity>(this Mock<T> mockStore, IStorageContext context = null) where T : class, IEntityStore<TEntity> where TEntity : Entity
        {
            if (context == null)
            {
                context = new Mock<IStorageContext>().Object;
            }

            mockStore.Setup(_ => _.Context).Returns(context);
        }
    }
}