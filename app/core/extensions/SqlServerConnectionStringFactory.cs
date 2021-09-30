namespace SignalBox.Core
{
    public static class SqlServerConnectionStringFactory
    {
        private static string ToLowercaseBoolean(bool v)
        {
            return v.ToString().ToLower();
        }
        public static string GenerateAzureSqlConnectionString(string serverName, string databaseName, string sqlServerUserName, string sqlServerPassword, bool persistSecurityInfo = false)
        {
            var cs = $"Server=tcp:{serverName}.database.windows.net,1433;Initial Catalog={databaseName};User ID={sqlServerUserName};Password={sqlServerPassword};Min Pool Size=0;Max Pool Size=30;Persist Security Info={ToLowercaseBoolean(persistSecurityInfo)};";

            return cs;
        }

    }
}