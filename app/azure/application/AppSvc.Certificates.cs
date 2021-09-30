
using System.Collections.Generic;
using Pulumi.AzureNative.CertificateRegistration;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;
using Pulumi.AzureNative.Resources;

namespace SignalBox.Azure
{
    partial class AppSvc : ComponentWithStorage
    {
        private void AppSvcCertificate(ResourceGroup rg)
        {
            var tenantId = azureConfig.Require("tenantId");
            var clientConfig = Pulumi.AzureNative.Authorization.GetClientConfig.InvokeAsync();
            clientConfig.Wait();
            var deployer = clientConfig.Result;
            var kv = new Vault("vault", new VaultArgs
            {
                ResourceGroupName = rg.Name,
                Tags = tags,
                Properties = new VaultPropertiesArgs
                {
                    TenantId = tenantId,
                    EnabledForDeployment = true,
                    EnabledForTemplateDeployment = true,
                    AccessPolicies = {
                        new AccessPolicyEntryArgs{
                            ObjectId = deployer.ObjectId,
                            ApplicationId = deployer.ClientId,
                            TenantId = tenantId,
                            Permissions = new PermissionsArgs
                            {
                                Certificates =
                                {
                                    Pulumi.Union<string, CertificatePermissions>.FromT1(CertificatePermissions.All)
                                },
                                Secrets =
                                {
                                    Pulumi.Union<string, SecretPermissions>.FromT1(SecretPermissions.All)
                                },
                                Keys =
                                {
                                    Pulumi.Union<string, KeyPermissions>.FromT1(KeyPermissions.All)
                                }
                            }
                        }
                    },
                    Sku = new Pulumi.AzureNative.KeyVault.Inputs.SkuArgs
                    {
                        Family = "A",
                        Name = SkuName.Standard,
                    },
                },
            });

            var certificateOrder = new AppServiceCertificateOrder("appSvcCert", new AppServiceCertificateOrderArgs
            {
                ResourceGroupName = rg.Name,
                Location = "global", // required because default location is invalid
                AutoRenew = environment == "Production",
                DistinguishedName = $"CN=*.{canonicalRootDomain}",
                ProductType = CertificateProductType.StandardDomainValidatedWildCardSsl,
                Tags = tags,
            });

            System.Console.WriteLine("Final certificate linking should be done in Az Portal");
        }
    }
}