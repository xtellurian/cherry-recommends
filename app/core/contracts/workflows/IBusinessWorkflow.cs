

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IBusinessWorkflow
    {
        Task<Business> CreateBusiness(string commonId, string name, string description);
        Task<BusinessMembership> AddToBusiness(string businessCommonId, Customer customer, Dictionary<string, object> properties = null);
    }
}