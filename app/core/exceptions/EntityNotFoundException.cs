using System;

namespace SignalBox.Core
{
    public class EntityNotFoundException : SignalBoxException
    {
        public EntityNotFoundException(Type t, string commonId, Exception ex) : base($"Entity of type {t.Name} with id {commonId} not found", ex)
        { }
        public EntityNotFoundException(Type t, long id, Exception ex) : base("Not Found", $"Entity of type {t.Name} with Id {id} not found", ex)
        { }
        public EntityNotFoundException(Type t, long id, string message) : base($"Entity of type {t.Name} with Id {id} not found", message)
        { }

        public override int Status => 404;
        public override string Title => "Resource Not Found";
    }
}