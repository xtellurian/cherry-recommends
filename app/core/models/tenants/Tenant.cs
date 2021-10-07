using System.Text.RegularExpressions;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace SignalBox.Core
{
    public class Tenant : Entity
    {
        protected Tenant() { }
        public Tenant(string name, string databaseName)
        {
            this.Name = name?.ToLower();
            this.DatabaseName = databaseName;
            Validate();
        }

        public override void Validate()
        {
            this.ValidateName(this.Name);
            this.ValidateName(this.DatabaseName);
            base.Validate();
        }

        private IEnumerable<string> reservedNames = new string[]
        {
            "www",
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

        private void ValidateName(string n)
        {
            if (!n.ContainsOnlyLowercaseAlphaNumeric('-', '_'))
            {
                throw new BadRequestException($"Value {n} has an invalid character. It should be lowercase alpha-numeric, underscore or dash.");
            }

            if (reservedNames.Any(_ => string.Equals(_, n)))
            {
                throw new BadRequestException($"{n} is a reserved word");
            }
            if (n.Length < 5)
            {
                throw new BadRequestException("Tenant names must be 5 or more characters");
            }
        }

        public string AccessScope()
        {
            return $"tenant:{Name}";
        }

        public string Name { get; set; }
        public string DatabaseName { get; set; }
    }
}