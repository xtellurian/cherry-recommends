using System;
using System.Collections.Generic;

namespace SignalBox.Core
{
#nullable enable
    // Pending Customer is used for messaging before the CUstomer is in the database.
    public class PendingCustomer
    {
        protected PendingCustomer()
        {
            CommonId = null!;
        }

        public PendingCustomer(string commonId, long? environmentId, string? name, bool overwriteExisting)
        {
            this.CommonId = commonId;
            this.EnvironmentId = environmentId;
            this.Name = name;
            OverwriteExisting = overwriteExisting;
        }
        public PendingCustomer(string commonId)
        {
            this.CommonId = commonId;
        }

        public bool OverwriteExisting { get; set; } = false;
        public string CommonId { get; set; }
        public long? EnvironmentId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public IntegratedSystemReference? IntegratedSystemReference { get; set; }
        public Dictionary<string, object>? Properties { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is PendingCustomer item))
            {
                return false;
            }

            return string.Equals(CommonId, item.CommonId) &&
                ((EnvironmentId == item.EnvironmentId) ||
                (EnvironmentId == null && item.EnvironmentId == null));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EnvironmentId, CommonId);
        }
    }
}