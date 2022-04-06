using Pulumi;
using Pulumi.Auth0;
using Pulumi.Auth0.Inputs;

namespace SignalBox.Azure
{
    class Auth0
    {
        public Auth0(Pulumi.AzureNative.Web.WebApp webApp)
        {
            var stackName = Deployment.Instance.StackName;
            var rootConfig = new Pulumi.Config();
            var canonicalRootDomain = new Pulumi.Config("appsvc").Get("canonical-root-domain");

            var apiResource = new ResourceServer("apiResource", new ResourceServerArgs
            {
                Name = $"{stackName}-CherryAPI",
                Identifier = Output.Format($"https://{webApp.DefaultHostName}"),
                SigningAlg = "RS256",
                TokenLifetime = 60 * 60 * 24 * 30, // 30 days for tokens from this endpoint
                TokenLifetimeForWeb = 60 * 60 * 12, // 1/2 day for tokens via browser
                EnforcePolicies = true, // enables RBAC for this API
                TokenDialect = "access_token_authz", // Add Permissions in the Access Token.
                SkipConsentForVerifiableFirstPartyClients = true,
                Scopes = {
                    // WARNING - DO NOT CHANGE THESE!
                    // When pulumi updates these scopes, any other scopes are deleted
                    // this is BAD because the tenant:TENANT_NAME scopes are all deleted
                    // meaning nobody has access to their tenants any more
                    new ResourceServerScopeArgs
                    {
                        Value = "webAPI"
                    },
                    new ResourceServerScopeArgs
                    {
                        Description = "Can read Customer metrics",
                        Value = Core.Security.Scopes.Metrics.Read
                    },
                    new ResourceServerScopeArgs
                    {
                        Description = "Can create and write to Customer Metrics",
                        Value = Core.Security.Scopes.Metrics.Write
                    }
                }
            });

            var m2mApp = new Client("m2mapp", new ClientArgs
            {
                Name = $"{stackName}-SignalBoxM2M",
                Description = "M2M App",
                AppType = "non_interactive",
                IsFirstParty = true,
                TokenEndpointAuthMethod = "client_secret_post",
                GrantTypes = {
                    "client_credentials",
                },
                ClientMetadata = new InputMap<object>
                {
                    {"project", Pulumi.Deployment.Instance.ProjectName},
                    {"stack", Pulumi.Deployment.Instance.StackName},
                    {"application", "m2mapp"}
                },
                JwtConfiguration = new ClientJwtConfigurationArgs
                {
                    Alg = "RS256",
                },
            });

            var m2mApiGrant = new ClientGrant("m2mAiGrant", new ClientGrantArgs
            {
                ClientId = m2mApp.ClientId,
                Audience = apiResource.Identifier!,
                Scopes = {
                    "webAPI",
                    Core.Security.Scopes.Metrics.Read,
                    Core.Security.Scopes.Metrics.Write,
                }
            });

            var hosts = new System.Collections.Generic.List<Pulumi.Output<string>>
            {
                Output.Format($"https://{webApp.DefaultHostName}")
            };
            var defaultLoginUri = string.IsNullOrEmpty(canonicalRootDomain)
                ? Output.Format($"https://{webApp.DefaultHostName}")
                : Output.Create($"https://manage.{canonicalRootDomain}");

            if (!string.IsNullOrEmpty(canonicalRootDomain))
            {
                hosts.Add(Output.Create($"https://{canonicalRootDomain}")); // allow the canonical root to login
                hosts.Add(Output.Create($"https://*.{canonicalRootDomain}")); // allow all subdomains (tenants + specials) to login
            }
            var clientApp = new Client("reactApp", new ClientArgs
            {
                Name = "Cherry Recommends",
                Description = $"The Cherry Recommends web application. ({stackName})",
                AppType = "spa",
                IsFirstParty = true,
                TokenEndpointAuthMethod = "none",
                Callbacks = hosts,
                WebOrigins = hosts,
                AllowedOrigins = hosts,
                InitiateLoginUri = defaultLoginUri,
                GrantTypes = {
                    "implicit",
                    "authorization_code",
                    "refresh_token"
                },
                AllowedLogoutUrls = hosts,
                ClientMetadata = new InputMap<object>
                {
                    {"project", Pulumi.Deployment.Instance.ProjectName},
                    {"stack", stackName},
                    {"application", "reactApp"}
                },
                JwtConfiguration = new ClientJwtConfigurationArgs
                {
                    Alg = "RS256",
                },
            });

            var adminRole = new Role("adminRole", new RoleArgs
            {
                Name = $"{stackName}-admin",
                Description = "Can work with behind the scenes admin resources, like Metrics and Models",
                Permissions = {
                    new RolePermissionArgs{
                        Name = "webAPI",
                        ResourceServerIdentifier = apiResource.Identifier!
                    },
                    new RolePermissionArgs{
                        Name = Core.Security.Scopes.Metrics.Read,
                        ResourceServerIdentifier = apiResource.Identifier!
                    },
                    new RolePermissionArgs{
                        Name = Core.Security.Scopes.Metrics.Write,
                        ResourceServerIdentifier = apiResource.Identifier!
                    }
                }
            });

            var normalRole = new Role("standardRole", new RoleArgs
            {
                Name = $"{stackName}-standard",
                Description = "The role all new client users should be assigned.",
                Permissions = {
                    new RolePermissionArgs{
                        Name = "webAPI",
                        ResourceServerIdentifier = apiResource.Identifier!
                    }
                },
            });

            this.Authority = $"https://{rootConfig.Require("login-domain")}";
            this.Audience = apiResource.Identifier!;
            this.DefaultAudience = apiResource.Identifier!;
            this.ReactDomain = rootConfig.Require("login-domain")!;
            this.ReactClientId = clientApp.ClientId;
            this.ReactManagementAudience = $"https://{Pulumi.Auth0.Config.Domain}/api/v2/";

            this.M2MClientId = m2mApp.ClientId;
            this.M2MClientSecret = m2mApp.ClientSecret;
            this.M2MEndpoint = $"https://{rootConfig.Require("login-domain")}/oauth/token";

        }

        public string Authority { get; }
        public Output<string> DefaultAudience { get; }
        public Output<string> Audience { get; }
        public string ReactDomain { get; }
        public Output<string> ReactClientId { get; }
        public string ReactManagementAudience { get; }
        public Output<string> M2MClientId { get; }
        public Output<string> M2MClientSecret { get; }
        public string M2MEndpoint { get; }
    }
}