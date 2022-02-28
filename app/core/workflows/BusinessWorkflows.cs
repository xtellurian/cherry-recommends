using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core.Metrics;
using SignalBox.Core.Metrics.Destinations;

namespace SignalBox.Core.Workflows
{
    public class BusinessWorkflows : IWorkflow
    {
        private readonly IBusinessStore businessStore;
        private readonly ILogger<BusinessWorkflows> logger;

        public BusinessWorkflows(IBusinessStore businessStore,
                                ILogger<BusinessWorkflows> logger)
        {
            this.businessStore = businessStore;
            this.logger = logger;
        }

        public async Task<Business> CreateBusiness(string commonId, string name, string description)
        {
            var business = await businessStore.Create(new Business(commonId, name, description));
            await businessStore.Context.SaveChanges();
            return business;
        }
    }
}