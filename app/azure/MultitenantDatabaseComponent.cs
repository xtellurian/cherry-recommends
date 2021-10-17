
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Sql;
using Pulumi.AzureNative.Sql.Inputs;

namespace SignalBox.Azure
{
    class MultitenantDatabaseComponent
    {
        public MultitenantDatabaseComponent(ResourceGroup rg)
        {
            var config = new Config();
            var username = config.Get("sqlAdmin") ?? "pulumi";
            var password = config.RequireSecret("sqlPassword");
            var sqlServer = new Server("multiSql", new ServerArgs
            {
                ResourceGroupName = rg.Name,
                AdministratorLogin = username,
                AdministratorLoginPassword = password,
                Version = "12.0",
            });

            var tenantDb = new Database("tenants", new DatabaseArgs
            {
                DatabaseName = "tenants",
                Location = sqlServer.Location,
                ResourceGroupName = rg.Name,
                ServerName = sqlServer.Name,
                Sku = new Pulumi.AzureNative.Sql.Inputs.SkuArgs
                {
                    Name = "S0",
                    Tier = "Standard",
                }
            });

            var elasticPool = new ElasticPool("pool", new ElasticPoolArgs
            {
                Location = sqlServer.Location,
                PerDatabaseSettings = new ElasticPoolPerDatabaseSettingsArgs
                {
                    MaxCapacity = 2,
                    MinCapacity = 0,
                },
                ResourceGroupName = rg.Name,
                ServerName = sqlServer.Name,
                Sku = new SkuArgs
                {
                    Capacity = 2,
                    Name = "GP_Gen5",
                    Tier = "GeneralPurpose",
                }
            });

            var database = new Database("single", new DatabaseArgs
            {
                DatabaseName = "single",
                Location = sqlServer.Location,
                ResourceGroupName = rg.Name,
                ServerName = sqlServer.Name,
                ElasticPoolId = elasticPool.Id,
            });

            var fisherStRule = new Pulumi.AzureNative.Sql.FirewallRule("mtFisher",
                new Pulumi.AzureNative.Sql.FirewallRuleArgs
                {
                    ResourceGroupName = rg.Name,
                    ServerName = sqlServer.Name,
                    StartIpAddress = IpAddresses.FisherSt,
                    EndIpAddress = IpAddresses.FisherSt,
                });

            var azureServicesRule = new Pulumi.AzureNative.Sql.FirewallRule("mtAzureServices", new FirewallRuleArgs
            {
                ResourceGroupName = rg.Name,
                ServerName = sqlServer.Name,
                StartIpAddress = "0.0.0.0", // this is shorthand for allowing Azure services
                EndIpAddress = "0.0.0.0",
            });

            this.TenantDbConnectionString = Output.Tuple<string, string, string>(sqlServer.Name, tenantDb.Name, password)
                .Apply(t =>
                {
                    (string server, string database, string pwd) = t;
                    return
                        $"Server=tcp:{server}.database.windows.net,1433;Initial Catalog={database};User ID={username};Password={pwd};Min Pool Size=0;Max Pool Size=30;Persist Security Info=true;";
                });

            this.UserName = username;
            this.Password = password;
            this.Server = sqlServer;
            this.ElasticPool = elasticPool;
            this.ServerName = sqlServer.Name;
            this.TenantDbName = tenantDb.Name;
            this.ResourceGroup = rg;
        }

        public ResourceGroup ResourceGroup { get; }
        public Output<string> TenantDbConnectionString { get; }
        public string UserName { get; }
        public Output<string> Password { get; }
        public Server Server { get; }
        public ElasticPool ElasticPool { get; }
        public Output<string> ServerName { get; }
        public Output<string> TenantDbName { get; }
    }
}