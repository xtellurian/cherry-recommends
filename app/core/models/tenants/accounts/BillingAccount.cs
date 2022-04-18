using System.Collections.Generic;

namespace SignalBox.Core.Accounts
{
#nullable enable
    public class BillingAccount : Entity
    {

        protected BillingAccount()
        {
            Tenants = null!;
        }

        /// <summary>
        /// Create a new billing account for a tenant
        /// </summary>
        /// <param name="tenant"></param>
        /// <param name="planType"></param>
        public BillingAccount(Tenant tenant, PlanTypes planType = PlanTypes.None)
        {
            this.Tenants = new List<Tenant> { tenant };
            PlanType = planType;
        }

        /// <summary>
        /// A billing account has many tenants, because there situations where a single biller will connect to multiple tenants.
        /// However, typically there will be only 1 tenant here.
        /// </summary>
        public ICollection<Tenant> Tenants { get; set; }

        public PlanTypes PlanType { get; set; }
    }
}