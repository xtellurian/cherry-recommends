using System;

namespace SignalBox.Core
{
    public class EntityNotFoundException : SignalBoxException
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }
        public EntityNotFoundException(Type entityType, long id) :
        base($"An entity of type {entityType.Name} with Id {id} was not found")
        {}
    }
}