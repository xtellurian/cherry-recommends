using SignalBox.Core;

namespace SignalBox.Web
{
    [System.AttributeUsage(System.AttributeTargets.Method | System.AttributeTargets.Class)]
    public class AllowTenantNullAttribute : System.Attribute
    {
        public AllowTenantNullAttribute()
        { }

    }
}