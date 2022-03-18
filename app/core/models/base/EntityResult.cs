namespace SignalBox.Core
{
    public class EntityResult<T> where T : Entity
    {
        public readonly bool Success = false;
        public readonly T Entity = null;
        public readonly SignalBoxException Exception = null;

        public EntityResult(T entity)
        {
            if (entity != null)
            {
                this.Success = true;
                this.Entity = entity;
            }
            else
            {
                this.Exception = new EntityNotFoundException(typeof(T));
            }
        }
    }
}