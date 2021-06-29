using System;

namespace SignalBox.Core
{
    public class EntityNotFoundException<T> : SignalBoxException where T : Entity
    {
        public EntityNotFoundException(string message) : base($"Entity of type {typeof(T).Name} not found", message)
        { }
        public EntityNotFoundException(long id) : base("Not Found", $"Entity of type {typeof(T).Name} with Id {id} not found")
        { }
        public EntityNotFoundException(long id, string message) : base($"Entity of type {typeof(T).Name} with Id {id} not found", message)
        { }
    }
}