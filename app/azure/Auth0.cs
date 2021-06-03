
using System;
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

            var apiResource = new ResourceServer("apiResource", new ResourceServerArgs
            {
                Name = $"{stackName}-SignalBoxAPI",
                Identifier = Output.Format($"https://{webApp.DefaultHostName}"),
                SigningAlg = "RS256",
                Scopes = {
                    new ResourceServerScopeArgs
                    {
                        Value = "webAPI"
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
                    "webAPI"
                }
            });

            var clientApp = new Client("reactApp", new ClientArgs
            {
                Name = "Signal Box",
                Description = "React Application frontend",
                AppType = "spa",
                IsFirstParty = true,
                TokenEndpointAuthMethod = "none",
                Callbacks = {
                    Output.Format($"https://{webApp.DefaultHostName}")
                },
                WebOrigins = {
                    Output.Format($"https://{webApp.DefaultHostName}")
                },
                AllowedOrigins = {
                    Output.Format($"https://{webApp.DefaultHostName}")
                },
                GrantTypes = {
                    "implicit",
                    "authorization_code",
                    "refresh_token"
                },
                AllowedLogoutUrls = {
                    Output.Format($"https://{webApp.DefaultHostName}")
                },
                ClientMetadata = new InputMap<object>
                {
                    {"project", Pulumi.Deployment.Instance.ProjectName},
                    {"stack", Pulumi.Deployment.Instance.StackName},
                    {"application", "reactApp"}
                },
                JwtConfiguration = new ClientJwtConfigurationArgs
                {
                    Alg = "RS256",
                },
            });

            this.Authority = $"https://{Pulumi.Auth0.Config.Domain}";
            this.Audience = apiResource.Identifier!;
            this.Audience = apiResource.Identifier!;
            this.ReactDomain = Pulumi.Auth0.Config.Domain!;
            this.ReactClientId = clientApp.ClientId;
            this.ReactManagementAudience = $"https://{Pulumi.Auth0.Config.Domain}/api/v2";

            this.M2MClientId = m2mApp.ClientId;
            this.M2MClientSecret = m2mApp.ClientSecret;
            this.M2MEndpoint = $"https://{Pulumi.Auth0.Config.Domain}/oauth/token";

        }

        public string Authority { get; }
        public Output<string> Audience { get; }
        public string ReactDomain { get; }
        public Output<string> ReactClientId { get; }
        public string ReactManagementAudience { get; }
        public Output<string> M2MClientId { get; }
        public Output<string> M2MClientSecret { get; }
        public string M2MEndpoint { get; }
    }
}