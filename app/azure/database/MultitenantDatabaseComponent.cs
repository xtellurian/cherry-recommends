using System.Collections.Generic;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Sql;
using Pulumi.AzureNative.Sql.Inputs;
using Pulumi.Random;
using SignalBox.Core.Constants;

namespace SignalBox.Azure
{
    class MultitenantDatabaseComponent : DatabaseComponentBase
    {
        public MultitenantDatabaseComponent(ResourceGroup rg) : base(rg)
        {
            var tenantDb = new Database("tenants", new DatabaseArgs
            {
                DatabaseName = "tenants",
                Location = sqlServer.Location,
                ResourceGroupName = rg.Name,
                ServerName = sqlServer.Name,
                Sku = new SkuArgs
                {
                    Name = "S0",
                    Tier = "Standard",
                }
            }, new CustomResourceOptions
            {
                Protect = string.Equals(config.Require("environment"), "Production") // protect the tenants DB in Production
            });

            var elasticPool = new ElasticPool("pool", new ElasticPoolArgs
            {
                Location = sqlServer.Location,
                PerDatabaseSettings = new ElasticPoolPerDatabaseSettingsArgs
                {
                    MaxCapacity = databaseConfig.GetInt32("poolMaxCapacityPerDatabase") ?? 2,
                    MinCapacity = 0,
                },
                ResourceGroupName = rg.Name,
                ServerName = sqlServer.Name,
                Sku = new SkuArgs
                {
                    Capacity = databaseConfig.GetInt32("poolCapacity") ?? 2,
                    Name = databaseConfig.Get("poolSkuName") ?? "GP_Gen5",
                    Tier = databaseConfig.Get("poolSkuTier") ?? "GeneralPurpose",
                }
            }, new CustomResourceOptions
            {
                Protect = string.Equals(config.Require("environment"), "Production") // protect the pool in Production
            });

            var database = new Database("single", new DatabaseArgs
            {
                DatabaseName = "single",
                Location = sqlServer.Location,
                ResourceGroupName = rg.Name,
                ServerName = sqlServer.Name,
                ElasticPoolId = elasticPool.Id,
            });

            var rules = new List<Pulumi.AzureNative.Sql.FirewallRule>();
            foreach (var ipAddress in IpAddresses.SqlServerWhitelist)
            {
                rules.Add(new Pulumi.AzureNative.Sql.FirewallRule(ipAddress.Key,
                    new Pulumi.AzureNative.Sql.FirewallRuleArgs
                    {
                        ResourceGroupName = rg.Name,
                        ServerName = sqlServer.Name,
                        StartIpAddress = ipAddress.Value,
                        EndIpAddress = ipAddress.Value,
                    }, new CustomResourceOptions
                    {
                        DeleteBeforeReplace = true
                    })
                );
            }

            var azureServicesRule = new Pulumi.AzureNative.Sql.FirewallRule("mtAzureServices", new FirewallRuleArgs
            {
                ResourceGroupName = rg.Name,
                ServerName = sqlServer.Name,
                StartIpAddress = "0.0.0.0", // this is shorthand for allowing Azure services
                EndIpAddress = "0.0.0.0",
            });

            this.TenantDbConnectionString = Output.Tuple<string, string, string>(sqlServer.Name, tenantDb.Name, Password)
                .Apply(t =>
                {
                    (string server, string database, string pwd) = t;
                    return
                        $"Server=tcp:{server}.database.windows.net,1433;Initial Catalog={database};User ID={UserName};Password={pwd};Min Pool Size=0;Max Pool Size=30;Persist Security Info=true;";
                });


            this.Server = sqlServer;
            this.ElasticPool = elasticPool;
            this.ServerName = sqlServer.Name;
            this.TenantDbName = tenantDb.Name;
            this.ResourceGroup = rg;

        }

        public ResourceGroup ResourceGroup { get; }
        public Output<string> TenantDbConnectionString { get; }
        public Server Server { get; }
        public ElasticPool ElasticPool { get; }
        public Output<string> ServerName { get; }
        public Output<string> TenantDbName { get; }
    }
}