using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SignalBox.Core.Accounts;

#nullable enable
namespace SignalBox.Core
{
    public class Tenant : Entity
    {
        public static string Status_Creating = "Creating";
        public static string Status_Database_Created = "DatabaseCreated";
        public static string Status_Created = "Created";

        protected Tenant()
        {
            this.Name = null!;
            this.DatabaseName = null!;
        }

        public Tenant(string name, string databaseName)
        {
            this.Name = name?.ToLower() ?? throw new BadRequestException("Tenant name cannot be null");
            this.DatabaseName = databaseName;
            this.Status = Status_Creating;
            Validate();
        }

        public override void Validate()
        {
            ValidateName(this.Name);
            ValidateName(this.DatabaseName);
            base.Validate();
        }

        private static IEnumerable<string> reservedNames => new string[]
        {
            "www",
            "manage",
            "tenant",
            "admin",
            "signup",
            "signin",
            "login",
            "authenticate",
            "logout",
            "administrator",
            "four2"
        };

        public static void ValidateName(string n)
        {
            if (n.StartsWith('_'))
            {
                throw new BadRequestException($"Tenant names must not start with an underscore.");
            }
            if (!n.ContainsOnlyLowercaseAlphaNumeric('-', '_'))
            {
                throw new BadRequestException($"Value {n} has an invalid character. It should be lowercase alpha-numeric, underscore or dash.");
            }

            if (reservedNames.Any(_ => string.Equals(_, n)))
            {
                throw new NameNotAvailableException(n);
            }
            if (n.Length < 4)
            {
                throw new BadRequestException("Tenant names must be 4 or more characters");
            }
        }

        public string AccessScope()
        {
            return $"tenant:{Name}";
        }

        public string MemberRoleName()
        {
            return $"tenant:{Name}";
        }

        public string Name { get; set; }
        public string DatabaseName { get; set; }
        public string? Status { get; set; }

        [JsonIgnore]
        public ICollection<TenantTermsOfServiceAcceptance>? AcceptedTerms { get; set; }
        [JsonIgnore]
        public long? AccountId { get; set; }
        [JsonIgnore]
        public BillingAccount? Account { get; set; }
    }
}