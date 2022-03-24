using System.Collections.Generic;

namespace SignalBox.Core.Constants
{
    public static class AzureDBUserNames
    {
        public const string AppAdminUserName = "pulumiappadmin";
        public const string AppReadUserName = "pulumiappread";
        public static Dictionary<string, string> AzureDBUserNameList = new Dictionary<string, string>
        {
            { AppAdminUserName, "db_datawriter" },
            { AppReadUserName, "db_datareader" }
        };
    }
}