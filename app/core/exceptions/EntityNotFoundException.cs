using System;

namespace SignalBox.Core
{
    public class EntityNotFoundException : SignalBoxException
    {
        public EntityNotFoundException(Type t, string commonId, Exception ex) : base($"Entity of type {t.Name} with id {commonId} not found", ex)
        {
            this._entityType = t;
        }
        public EntityNotFoundException(Type t, long id, Exception ex) : base("Not Found", $"Entity of type {t.Name} with Id {id} not found", ex)
        {
            this._entityType = t;
        }
        public EntityNotFoundException(Type t, long id, string message) : base($"Entity of type {t.Name} with Id {id} not found", message)
        {
            this._entityType = t;
        }

        private Type _entityType;
        public override int Status => 404;
        public override string Title => $"{_entityType?.Name} Resource Not Found";
    }
}