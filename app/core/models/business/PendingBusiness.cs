namespace SignalBox.Core
{
    using System;
    using System.Collections.Generic;

#nullable enable
    // Pending Customer is used for messaging before the CUstomer is in the database.
    public class PendingBusiness
    {
        protected PendingBusiness()
        {
            CommonId = null!;
        }

        public PendingBusiness(string commonId, long? environmentId, string? name, bool overwriteExisting)
        {
            CommonId = commonId;
            EnvironmentId = environmentId;
            Name = name;
            OverwriteExisting = overwriteExisting;
        }
        public PendingBusiness(string commonId)
        {
            this.CommonId = commonId;
        }

        public bool OverwriteExisting { get; set; } = false;
        public string CommonId { get; set; }
        public long? EnvironmentId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Dictionary<string, object>? Properties { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is PendingBusiness item))
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