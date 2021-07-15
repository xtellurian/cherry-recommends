using System;

namespace SignalBox.Core
{
    public class EntityNotFoundException : SignalBoxException
    {
        public EntityNotFoundException(Type t, string commonId, Exception ex) : base($"Entity of type {t.Name} with id {commonId} not found", ex)
        {
            this._entityType = t;
            this._entityId = commonId;
        }
        public EntityNotFoundException(Type t, long id, Exception ex) : base("Not Found", $"Entity of type {t.Name} with Id {id} not found", ex)
        {
            this._entityType = t;
            this._entityId = id.ToString();
        }
        public EntityNotFoundException(Type t, long id, string message) : base($"Entity of type {t.Name} with Id {id} not found", message)
        {
            this._entityType = t;
            this._entityId = id.ToString();
        }

        private Type _entityType;
        private string _entityId;
        public override int Status => 404;
        public override string Title => $"{_entityType?.Name} ({_entityId}) Resource Not Found";
    }
}