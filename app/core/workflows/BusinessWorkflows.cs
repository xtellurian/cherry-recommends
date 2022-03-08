using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Core.Workflows
{
    public class BusinessWorkflows : IWorkflow, IBusinessWorkflow
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

        public async Task<Business> CreateOrUpdate(PendingBusiness pendingBusiness, bool saveOnComplete = true)
        {
            Business business;
            if (await businessStore.ExistsFromCommonId(pendingBusiness.CommonId))
            {
                business = await businessStore.ReadFromCommonId(pendingBusiness.CommonId);
                if (pendingBusiness.OverwriteExisting)
                {
                    business.Name = pendingBusiness.Name;
                    business.Description = pendingBusiness.Description;
                    business.Properties = new DynamicPropertyDictionary(pendingBusiness.Properties);
                }
            }
            else
            {
                business = await businessStore.Create(new Business(pendingBusiness.CommonId, pendingBusiness.Name, pendingBusiness.Description));
                if (saveOnComplete)
                {
                    await storageContext.SaveChanges();
                }
            }
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

        /// <summary>
        /// Adds a customer to a business. Creates the business if it doesn't exist.
        /// </summary>
        /// <param name="businessCommonId">Lookuop this business. Create if not exist.</param>
        /// <param name="customer">The customer to add to the Business</param>
        /// <param name="properties">Optional. Add these properties to the businesses. </param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<BusinessMembership> AddToBusiness(string businessCommonId, Customer customer, Dictionary<string, object> properties = null)
        {
            Business business;
            if (customer.BusinessMembership?.BusinessId != null)
            {
                var currentBusiness = await businessStore.Read(customer.BusinessMembership.BusinessId);
                if (!string.Equals(businessCommonId, currentBusiness.CommonId))
                {
                    logger.LogWarning($"Customer {customer.Id} is changing businesses");
                }
            }

            if (await businessStore.ExistsFromCommonId(businessCommonId))
            {
                business = await businessStore.ReadFromCommonId(businessCommonId);
            }
            else
            {
                business = await businessStore.Create(new Business(businessCommonId));
            }

            customer.BusinessMembership = new BusinessMembership
            {
                BusinessId = business.Id,
                Business = business,
                CustomerId = customer.Id,
                Customer = customer
            };
            business.Properties ??= new DynamicPropertyDictionary();
            business.Properties.Merge(properties);

            await businessStore.Context.SaveChanges();
            return customer.BusinessMembership;
        }
    }
}