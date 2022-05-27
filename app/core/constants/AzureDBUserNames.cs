using System.Collections.Generic;

namespace SignalBox.Core.Constants
{
    public static class AzureDBUserNames
    {
        public const string AppAdminUserName = "pulumiappadmin";
        public const string AppReadUserName = "pulumiappread";
        public static IEnumerable<DbCredential> AzureDBCredentials = new List<DbCredential>()
        {
            new DbCredential
            {
                UserName = AppAdminUserName,
                Roles = new [] {
                    "db_datawriter", "db_datareader"
                }
            },
            new DbCredential
            {
                UserName = AppReadUserName,
                Roles = new [] { "db_datareader" }
            }
        };
    }

    public struct DbCredential
    {
        public string UserName { get; set; }
        public string[] Roles { get; set; }
    }
}