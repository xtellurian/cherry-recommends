using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Core.Workflows
{
    public class BusinessWorkflows : IWorkflow
    {
        private readonly IStorageContext storageContext;
        private readonly IBusinessStore businessStore;
        private readonly ILogger<BusinessWorkflows> logger;

        public BusinessWorkflows(IStorageContext storageContext,
                                IBusinessStore businessStore,
                                ILogger<BusinessWorkflows> logger)
        {
            this.storageContext = storageContext;
            this.businessStore = businessStore;
            this.logger = logger;
        }

        public async Task<Business> CreateBusiness(string commonId, string name, string description)
        {
            var business = await businessStore.Create(new Business(commonId, name, description));
            await storageContext.SaveChanges();
            return business;
        }
    }
}