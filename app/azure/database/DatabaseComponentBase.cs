using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Sql;
using Pulumi.Random;
using SignalBox.Core.Constants;

namespace SignalBox.Azure
{
    public abstract class DatabaseComponentBase
    {
        protected Config config;
        protected Config databaseConfig;
        protected Server sqlServer;

        public DatabaseComponentBase(ResourceGroup rg)
        {
            this.config = new Config();
            this.databaseConfig = new Config("database");
            var username = config.Get("sqlAdmin") ?? "pulumi";
            var password = config.RequireSecret("sqlPassword");
            var adminusername = AzureDBUserNames.AppAdminUserName;
            var readusername = AzureDBUserNames.AppReadUserName;
            var adminpassword = new RandomPassword(adminusername, new RandomPasswordArgs
            {
                Length = 16,
                MinLower = 2,
                MinNumeric = 2,
                MinSpecial = 2,
                MinUpper = 2,
                OverrideSpecial = "!$",
            });
            var readpassword = new RandomPassword(readusername, new RandomPasswordArgs
            {
                Length = 16,
                MinLower = 2,
                MinNumeric = 2,
                MinSpecial = 2,
                MinUpper = 2,
                OverrideSpecial = "!$",
            });

            this.sqlServer = new Server("multiSql", new ServerArgs
            {
                ResourceGroupName = rg.Name,
                AdministratorLogin = username,
                AdministratorLoginPassword = password,
                Version = "12.0",
            }, new CustomResourceOptions
            {
                Protect = string.Equals(config.Require("environment"), "Production") // protect the SQL Server
            });

            this.UserName = username;
            this.Password = password;
            this.AdminUserName = adminusername;
            this.AdminPassword = adminpassword.Result;
            this.ReadUserName = readusername;
            this.ReadPassword = readpassword.Result;
        }
        public string UserName { get; }
        public Output<string> Password { get; }
        public string AdminUserName { get; }
        public Output<string> AdminPassword { get; }
        public string ReadUserName { get; }
        public Output<string> ReadPassword { get; }
    }
}