namespace SignalBox.Core
{
    public static class SqlLiteConnectionStringFactory
    {
        private static string GenerateConnectionString(string directory, string dbName)
        {
            return $"DataSource={directory}/{dbName}.db;Cache=Shared";
        }
        public static string GenerateConnectionString(string appDir, string directory, string dbName)
        {
            if (string.IsNullOrEmpty(appDir))
            {
                return GenerateConnectionString(directory, dbName);
            }
            else
            {
                return $"DataSource={appDir}/web/{directory}/{dbName}.db;Cache=Shared";
            }
        }
    }
}