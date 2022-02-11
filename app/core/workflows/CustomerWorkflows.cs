
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

#nullable enable
namespace SignalBox.Core.Workflows
{
    public class CustomerWorkflows : IWorkflow
    {
        private readonly IStorageContext storageContext;
        private readonly ILogger<CustomerWorkflows> logger;
        private readonly ICustomerStore customerStore;
        private readonly ITrackedUserSystemMapStore trackedUserSystemMapStore;
        private readonly IIntegratedSystemStore integratedSystemStore;
        private readonly IDateTimeProvider dateTimeProvider;

        public CustomerWorkflows(IStorageContext storageContext,
            ILogger<CustomerWorkflows> logger,
            ICustomerStore customerStore,
            ITrackedUserSystemMapStore trackedUserSystemMapStore,
            IIntegratedSystemStore integratedSystemStore,
            IDateTimeProvider dateTimeProvider)
        {
            this.storageContext = storageContext;
            this.logger = logger;
            this.customerStore = customerStore;
            this.trackedUserSystemMapStore = trackedUserSystemMapStore;
            this.integratedSystemStore = integratedSystemStore;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<IEnumerable<Customer>> CreateIfNotExist(IEnumerable<PendingCustomer> pendingCustomers)
        {
            var newCustomers = await customerStore.CreateIfNotExists(pendingCustomers);

            foreach (var u in newCustomers)
            {
                u.LastUpdated = dateTimeProvider.Now;
            }

            await storageContext.SaveChanges();
            return newCustomers;
        }
        public async Task<Customer> MergeUpdateProperties(Customer customer, IDictionary<string, object> properties, long? integratedSystemId = null, bool? saveOnComplete = true)
        {
            var newProperties = new DynamicPropertyDictionary(properties);

            customer.Properties = newProperties;
            customer.LastUpdated = dateTimeProvider.Now;

            if (saveOnComplete == true)
            {
                await storageContext.SaveChanges();
            }
            return customer;
        }

        public async Task<Customer> CreateOrUpdate(string commonUserId,
                                                    string? name,
                                                    Dictionary<string, object>? properties,
                                                    long? integratedSystemId,
                                                    string? integratedSystemUserId,
                                                    bool saveOnComplete = true)
        {
            Customer customer;
            if (await customerStore.ExistsFromCommonId(commonUserId))
            {
                customer = await customerStore.ReadFromCommonId(commonUserId, _ => _.IntegratedSystemMaps);
                logger.LogInformation($"Updating user {customer.Id}");
                if (!string.IsNullOrEmpty(name))
                {
                    customer.Name = name;
                }
                if (properties != null && properties.Keys.Count > 0)
                {
                    customer = await MergeUpdateProperties(customer, properties, integratedSystemId, saveOnComplete: false);
                }
            }
            else
            {
                customer = await customerStore.Create(new Customer(commonUserId, name, new DynamicPropertyDictionary(properties)));
                logger.LogInformation($"Created user {customer.Id}");
            }

            if (integratedSystemId.HasValue && !customer.IntegratedSystemMaps.Any(_ => _.IntegratedSystemId == integratedSystemId))
            {
                logger.LogInformation($"Connecting user to integrated system: {integratedSystemId}");
                var integratedSystem = await integratedSystemStore.Read(integratedSystemId.Value);
                await trackedUserSystemMapStore.Create(new TrackedUserSystemMap(integratedSystemUserId, integratedSystem, customer));
            }
            else
            {
                logger.LogWarning($"Not setting integratedSystemId for tracked user {customer.Id}");
            }


            if (saveOnComplete == true)
            {
                await storageContext.SaveChanges();
            }

            return customer;
        }

        public async Task<IEnumerable<Customer>> CreateOrUpdateMultiple(
            IEnumerable<CreateOrUpdateCustomerModel> newUsers)
        {
            // check for incoming duplicates
            var numDistinct = newUsers.Select(_ => _.CustomerId).Distinct().Count();
            if (numDistinct < newUsers.Count())
            {
                throw new BadRequestException("All CommonUserId must be unique when batch creating or updating.");
            }
            var users = new List<Customer>();
            foreach (var u in newUsers)
            {
                var user = await CreateOrUpdate(u.CustomerId, u.Name, u.Properties, u.IntegratedSystemId, u.IntegratedSystemUserId, false);
            }

            await storageContext.SaveChanges();
            return users;
        }

        public struct CreateOrUpdateCustomerModel
        {
            public CreateOrUpdateCustomerModel(string customerId,
                                             string? name,
                                             Dictionary<string, object>? properties,
                                             long? integratedSystemId,
                                             string? integratedSystemUserId)
            {
                CustomerId = customerId;
                Name = name;
                Properties = properties;
                IntegratedSystemId = integratedSystemId;
                IntegratedSystemUserId = integratedSystemUserId;
            }

            public string CustomerId { get; set; }
            public string? Name { get; set; }
            public Dictionary<string, object>? Properties { get; set; }
            public long? IntegratedSystemId { get; set; }
            public string? IntegratedSystemUserId { get; set; }
        }
    }
}