

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IBusinessWorkflow
    {
        Task<Business> CreateBusiness(string commonId, string name, string description);
        Task<Business> CreateOrUpdate(PendingBusiness pendingBusiness, bool saveOnComplete = true);
        Task<BusinessMembership> AddToBusiness(string businessCommonId, Customer customer, Dictionary<string, object> properties = null);
    }
}