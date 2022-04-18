namespace SignalBox.Core
{
    public class TenantNotFoundException : SignalBoxException
    {
        public TenantNotFoundException(string name) : base($"Tenant {name} not found")
        { }
        public TenantNotFoundException(long id, System.Exception inner) : base($"Tenant Id = {id} not found", inner)
        { }
    }
}