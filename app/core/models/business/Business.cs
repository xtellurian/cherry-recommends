using System.Collections.Generic;

namespace SignalBox.Core
{
    public class Business : CommonEntity
    {
        protected override int CommonIdMinLength => 1;

        protected Business()
        { }

        public Business(string businessId) : base(businessId, null)
        { }

        public Business(string businessId, string businessName) : base(businessId, businessName)
        { }

        public string Description { get; set; }

    }
}
