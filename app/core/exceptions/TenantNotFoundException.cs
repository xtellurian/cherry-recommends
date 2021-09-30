namespace SignalBox.Core
{
    public class TenantNotFoundException : SignalBoxException
    {
        public TenantNotFoundException(string name) : base($"Tenant {name} not found")
        { }
    }
}