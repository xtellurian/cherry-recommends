namespace SignalBox.Core
{
    public static class SqlServerConnectionStringFactory
    {
        private static string ToLowercaseBoolean(bool v)
        {
            return v.ToString().ToLower();
        }
        public static string GenerateAzureSqlConnectionString(string serverName, string databaseName, string sqlServerUserName, string sqlServerPassword, int maxPoolSize = 30, bool persistSecurityInfo = false)
        {
            var cs = $"Server=tcp:{serverName}.database.windows.net,1433;Initial Catalog={databaseName};User ID={sqlServerUserName};Password={sqlServerPassword};Min Pool Size=0;Max Pool Size={maxPoolSize};Persist Security Info={ToLowercaseBoolean(persistSecurityInfo)};";

            return cs;
        }

        public static string GenerateLocalSqlConnectionString(string databaseName, string sqlServerUserName, string sqlServerPassword)
        {
            var cs = $"Server=127.0.0.1,1433;Database={databaseName};User Id={sqlServerUserName};Password={sqlServerPassword}";

            return cs;
        }

    }
}