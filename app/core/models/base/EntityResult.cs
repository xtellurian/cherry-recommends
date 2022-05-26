namespace SignalBox.Core
{
#nullable enable
    public class EntityResult<T> where T : Entity
    {
        public bool Success => Entity != null;
        public readonly T? Entity = null;
        public readonly SignalBoxException? Exception = null;

        public EntityResult(T? entity)
        {
            if (entity != null)
            {
                this.Entity = entity;
            }
            else
            {
                this.Exception = new EntityNotFoundException(typeof(T));
            }
        }
    }
}