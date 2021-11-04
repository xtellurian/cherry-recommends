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
            var auth0Config = new Pulumi.Config("auth0");
            var canonicalRootDomain = new Pulumi.Config("appsvc").Get("canonical-root-domain");

            var apiResource = new ResourceServer("apiResource", new ResourceServerArgs
            {
                Name = $"{stackName}-SignalBoxAPI",
                Identifier = Output.Format($"https://{webApp.DefaultHostName}"),
                SigningAlg = "RS256",
                TokenLifetime = 60 * 60 * 24 * 30, // 30 days for tokens from this endpoint
                TokenLifetimeForWeb = 60 * 60 * 12, // 1/2 day for tokens via browser
                EnforcePolicies = true, // enables RBAC for this API
                Scopes = {
                    new ResourceServerScopeArgs
                    {
                        Value = "webAPI"
                    },
                    new ResourceServerScopeArgs
                    {
                        Description = "Can read Tracked User features",
                        Value = Core.Security.Scopes.Features.Read
                    },
                    new ResourceServerScopeArgs
                    {
                        Description = "Can create and write to Tracked User Features",
                        Value = Core.Security.Scopes.Features.Write
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
                    Core.Security.Scopes.Features.Read,
                    Core.Security.Scopes.Features.Write,
                }
            });

            var hosts = new InputList<string>
            {
                Output.Format($"https://{webApp.DefaultHostName}")
            };
            if (!string.IsNullOrEmpty(canonicalRootDomain))
            {
                hosts.Add($"https://{canonicalRootDomain}"); // allow the canonical root to login
                hosts.Add($"https://*.{canonicalRootDomain}"); // allow all subdomains (tenants + specials) to login
            }
            var clientApp = new Client("reactApp", new ClientArgs
            {
                Name = "Signal Box",
                Description = "React Application frontend",
                AppType = "spa",
                IsFirstParty = true,
                TokenEndpointAuthMethod = "none",
                Callbacks = hosts,
                WebOrigins = hosts,
                AllowedOrigins = hosts,
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
                Description = "Can work with behind the scenes admin resources, like Features and Models",
                Permissions = {
                    new RolePermissionArgs{
                        Name = "webAPI",
                        ResourceServerIdentifier = apiResource.Identifier!
                    },
                    new RolePermissionArgs{
                        Name = Core.Security.Scopes.Features.Read,
                        ResourceServerIdentifier = apiResource.Identifier!
                    },
                    new RolePermissionArgs{
                        Name = Core.Security.Scopes.Features.Write,
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

            this.Authority = $"https://{Pulumi.Auth0.Config.Domain}";
            this.Audience = apiResource.Identifier!;
            this.DefaultAudience = apiResource.Identifier!;
            this.ReactDomain = Pulumi.Auth0.Config.Domain!;
            this.ReactClientId = clientApp.ClientId;
            this.ReactManagementAudience = $"https://{Pulumi.Auth0.Config.Domain}/api/v2";

            this.M2MClientId = m2mApp.ClientId;
            this.M2MClientSecret = m2mApp.ClientSecret;
            this.M2MEndpoint = $"https://{Pulumi.Auth0.Config.Domain}/oauth/token";

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