using System.Linq;
using System.Reflection;
namespace SignalBox.Infrastructure
{
    public static class VersionInfo
    {
        public static string GitHash => Assembly
                .GetAssembly(typeof(VersionInfo))
                .GetCustomAttributes<AssemblyMetadataAttribute>()
                .FirstOrDefault(attr => attr.Key == "GitHash")?.Value;
    }
}