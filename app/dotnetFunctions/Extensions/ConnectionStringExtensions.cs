using System.IO;

namespace SignalBox.Functions
{
    public static class ConnectionStringExtensions
    {
        public static string FixSqliteConnectionString(this string cs)
        {
            var result =  cs.Replace("{AppDir}", Directory.GetCurrentDirectory()).Replace("dotnetFunctions/bin/output/", "");

            return result;
        }
    }
}