using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using Pulumi.AzureNative.Web;

class DocsStack : Stack
{
    public DocsStack()
    {
        // Create an Azure Resource Group
        var resourceGroup = new ResourceGroup("docs", new ResourceGroupArgs
        {
            ResourceGroupName = $"docs-{Pulumi.Deployment.Instance.StackName}"
        });

        // Create an Azure resource (Storage Account)
        var storageAccount = new StorageAccount("docshost", new StorageAccountArgs
        {
            ResourceGroupName = resourceGroup.Name,
            Sku = new SkuArgs
            {
                Name = SkuName.Standard_LRS
            },
            EnableHttpsTrafficOnly = false,
            Kind = Kind.StorageV2,
        });

        var staticWebsite = new StorageAccountStaticWebsite("docs-web", new StorageAccountStaticWebsiteArgs
        {
            AccountName = storageAccount.Name,
            ResourceGroupName = resourceGroup.Name,
            IndexDocument = "index.html",
            Error404Document = "404.html",
        });

        this.StaticEndpoint = storageAccount.PrimaryEndpoints.Apply(primaryEndpoints => primaryEndpoints.Web);
        this.StorageAccountName = storageAccount.Name;

        // Export the primary key of the Storage Account
        this.PrimaryStorageKey = Output.Tuple(resourceGroup.Name, storageAccount.Name).Apply(names =>
            Output.CreateSecret(GetStorageAccountPrimaryKey(names.Item1, names.Item2)));
    }

    [Output]
    public Output<string> StaticEndpoint { get; set; }
    [Output]
    public Output<string> StorageAccountName { get; private set; }
    [Output]
    public Output<string> PrimaryStorageKey { get; set; }

    private static async Task<string> GetStorageAccountPrimaryKey(string resourceGroupName, string accountName)
    {
        var accountKeys = await ListStorageAccountKeys.InvokeAsync(new ListStorageAccountKeysArgs
        {
            ResourceGroupName = resourceGroupName,
            AccountName = accountName
        });
        return accountKeys.Keys[0].Value;
    }

    private static string GetContentType(string fileName)
    {
        if (fileName.EndsWith(".html"))
        {
            return "text/html";
        }
        else
        {
            return "application/octet-stream";
        }
    }

    private static List<string> FindAllFiles(string rootDir)
    {
        var pathsToSearch = new Queue<string>();
        var foundFiles = new List<string>();

        pathsToSearch.Enqueue(rootDir);

        while (pathsToSearch.Count > 0)
        {
            var dir = pathsToSearch.Dequeue();

            try
            {
                var files = Directory.GetFiles(dir);
                foreach (var file in Directory.GetFiles(dir))
                {
                    foundFiles.Add(file);
                }

                foreach (var subDir in Directory.GetDirectories(dir))
                {
                    pathsToSearch.Enqueue(subDir);
                }

            }
            catch (Exception /* TODO: catch correct exception */)
            {
                // Swallow.  Gulp!
            }
        }

        return foundFiles.ToList();
    }

}
