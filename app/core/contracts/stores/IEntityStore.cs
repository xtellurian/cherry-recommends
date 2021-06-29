namespace SignalBox.Core
{
    public interface IEntityStore<T> : IStore<T> where T : Entity
    {
        IStorageContext Context { get; }
    }
}