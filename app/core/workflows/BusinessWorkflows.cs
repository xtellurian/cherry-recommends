using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Core.Workflows
{
    public class BusinessWorkflows : IWorkflow
    {
        private readonly IStorageContext storageContext;
        private readonly IBusinessStore businessStore;
        private readonly ICustomerStore customerStore;
        private readonly ILogger<BusinessWorkflows> logger;

        public BusinessWorkflows(IStorageContext storageContext,
                                IBusinessStore businessStore,
                                ICustomerStore customerStore,
                                ILogger<BusinessWorkflows> logger)
        {
            this.storageContext = storageContext;
            this.businessStore = businessStore;
            this.customerStore = customerStore;
            this.logger = logger;
        }

        public async Task<Business> CreateBusiness(string commonId, string name, string description)
        {
            var business = await businessStore.Create(new Business(commonId, name, description));
            await storageContext.SaveChanges();
            return business;
        }
        
        public async Task<Customer> RemoveBusinessMembership(Business business, long customerId)
        {
            var customer = await customerStore.Read(customerId);
            if (customer == null)
            {
                throw new BadRequestException($"Customer Id {customerId} does not exist");
            }

            if (customer.BusinessMembership?.BusinessId != business.Id)
            {
                throw new BadRequestException($"Customer Id {customerId} is not a member of Business Id {business.Id}");
            }

            customer.BusinessMembership = null;
            await storageContext.SaveChanges();
            return customer;
        }
    }
}