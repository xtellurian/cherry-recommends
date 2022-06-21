using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Pulumi;
using Pulumi.Aws;
using Pulumi.Aws.Iam;
using Pulumi.Aws.S3;
using Pulumi.Aws.Ses;
using SignalBox.Core;

namespace Cherry.CloudInfrastructure.AWS
{
    class AwsInfrastructure : Stack
    {

        public AwsInfrastructure()
        {
            var config = new Pulumi.Config("aws");
            var region = config.Require("region");
            var emailUser = new User("emailUser", new UserArgs
            {
                Path = "/system/",
            });

            var accessKey = new AccessKey("emailUserAccessKey", new AccessKeyArgs
            {
                User = emailUser.Name,
                // PgpKey = "keybase:some_person_that_exists",
            });


            var policy = new UserPolicy("sendEmailPolicy", new UserPolicyArgs
            {
                User = emailUser.Name,
                Policy = JsonSerializer.Serialize(new Dictionary<string, object?>
                {

                    { "Version", "2012-10-17" },
                    { "Statement", new[]
                        {
                            new Dictionary<string, object?>
                            {
                                { "Action", "ses:SendRawEmail" },
                                { "Effect", "Allow" },
                                { "Resource", "*" },
                            },
                        }
                    },
                }),
            });

            var domainIdentity = new DomainIdentity("domainId", new DomainIdentityArgs { Domain = "cherry.ai" });

            var domainDkim = new DomainDkim("domainDkim", new DomainDkimArgs { Domain = domainIdentity.Domain });

            // Export the name of the bucket
            this.Email = Output.Create("hello@cherry.ai");
            this.SmtpPassword = accessKey.SesSmtpPasswordV4;
            this.SmtpUsername = accessKey.Id;
            this.Region = Output.Create(region);
            this.DomainVerificationToken = domainIdentity.VerificationToken;
            this.DkimTokens = domainDkim.DkimTokens;
        }

        [Output]
        public Output<string> Email { get; set; }
        [Output]
        public Output<string> SmtpPassword { get; set; }
        [Output]
        public Output<string> SmtpUsername { get; set; }
        [Output]
        public Output<string> Region { get; set; }
        [Output]
        public Output<string> DomainVerificationToken { get; set; }
        [Output]
        public Output<ImmutableArray<string>> DkimTokens { get; set; }
    }
}
