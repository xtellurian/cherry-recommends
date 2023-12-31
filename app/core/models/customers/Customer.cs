﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class Customer : CommonEntity
    {
        private const string anonymousCommonId = "anonymous";
        private const long anonymousId = -1;
        public static string AnonymousCommonId => anonymousCommonId;
        public static Customer Anonymous => new Customer(anonymousCommonId, "Anonymous Customer")
        {
            Id = anonymousId // need to include the primary key for EF Core reasons
        };

        protected override int CommonIdMinLength => 1;
        protected Customer()
        { }

        public Customer(string customerId) : base(customerId, null)
        { }

        public Customer(string customerId, string name) : base(customerId, name)
        { }

        public Customer(string customerId, string name, DynamicPropertyDictionary properties) : this(customerId, name)
        {
            if (properties != null)
            {
                this.Properties = properties;
            }
        }

        public Customer(string customerId, string name, IDictionary<string, object> properties)
        : this(customerId, name, new DynamicPropertyDictionary(properties))
        { }
        public Customer(string customerId, string name, IDictionary<string, string> properties)
        : this(customerId, name, new DynamicPropertyDictionary(properties))
        { }

        public string CommonUserId => CommonId;
        public string CustomerId => CommonId;
#nullable enable
        public string? Email { get; set; }

        [JsonIgnore]
        public ICollection<CustomerSegment> Segments { get; set; }

        [JsonIgnore]
        public ICollection<HistoricCustomerMetric> HistoricCustomerMetrics { get; set; }
        // this can be serialised and sent out
        public ICollection<TrackedUserSystemMap> IntegratedSystemMaps { get; set; } = new List<TrackedUserSystemMap>(); // for initialising

        public BusinessMembership? BusinessMembership { get; set; }
    }
}
