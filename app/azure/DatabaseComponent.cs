
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Sql;

namespace SignalBox.Azure
{
    class DatabaseComponent
    {
        public DatabaseComponent(ResourceGroup rg)
        {
            var config = new Config();
            var username = config.Get("sqlAdmin") ?? "pulumi";
            var password = config.RequireSecret("sqlPassword");
            var sqlServer = new Server("sqlserver", new ServerArgs
            {
                ResourceGroupName = rg.Name,
                AdministratorLogin = username,
                AdministratorLoginPassword = password,
                Version = "12.0",
            });

            var database = new Database("db", new DatabaseArgs
            {
                Location = sqlServer.Location,
                ResourceGroupName = rg.Name,
                ServerName = sqlServer.Name,
                Sku = new Pulumi.AzureNative.Sql.Inputs.SkuArgs
                {
                    Name = "S0"
                }
            });
            var fisherStRule = new Pulumi.AzureNative.Sql.FirewallRule("fisherst",
                new Pulumi.AzureNative.Sql.FirewallRuleArgs
                {
                    ResourceGroupName = rg.Name,
                    ServerName = sqlServer.Name,
                    StartIpAddress = "149.167.60.108",
                    EndIpAddress = "149.167.60.108",
                });

            var azureServicesRule = new Pulumi.AzureNative.Sql.FirewallRule("azureServices", new FirewallRuleArgs
            {
                ResourceGroupName = rg.Name,
                ServerName = sqlServer.Name,
                StartIpAddress = "0.0.0.0", // this is shorthand for allowing Azure services
                EndIpAddress = "0.0.0.0",
            });

            this.DatabaseConnectionString = Output.Tuple<string, string, string>(sqlServer.Name, database.Name, password)
                .Apply(t =>
                {
                    (string server, string database, string pwd) = t;
                    return
                        $"Server=tcp:{server}.database.windows.net,1433;Initial Catalog={database};User ID={username};Password={pwd};Min Pool Size=0;Max Pool Size=30;Persist Security Info=true;";
                });

            this.UserName = username;
            this.Password = password;
            this.ServerName = sqlServer.Name;
            this.DatabaseName = database.Name;
        }

        public Output<string> DatabaseConnectionString { get; }
        public string UserName { get; }
        public Output<string> Password { get; }
        public Output<string> ServerName { get; }
        public Output<string> DatabaseName { get; }
    }
}