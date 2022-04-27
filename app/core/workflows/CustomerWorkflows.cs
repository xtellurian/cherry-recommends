
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalBox.Core;

#nullable enable
namespace SignalBox.Core.Workflows
{
    public class CustomerWorkflows : IWorkflow, ICustomerWorkflow
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

        // public async Task<IEnumerable<Customer>> CreateIfNotExist(IEnumerable<PendingCustomer> pendingCustomers)
        // {
        //     var newCustomers = await customerStore.CreateIfNotExists(pendingCustomers);

        //     foreach (var u in newCustomers)
        //     {
        //         u.LastUpdated = dateTimeProvider.Now;
        //     }

        //     await storageContext.SaveChanges();
        //     return newCustomers;
        // }
        public async Task<Customer> MergeUpdateProperties(Customer customer, IDictionary<string, object> properties, bool? saveOnComplete = true)
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

        /// <summary>
        /// Deals with environments explicitly
        /// </summary>
        /// <param name="pendingCustomer">The customer data to create or retrieve</param>
        /// <param name="saveOnComplete">Should the database be updated by this method.</param>
        /// <returns>An existing or new customer.</returns>
        public async Task<Customer> CreateOrUpdate(PendingCustomer pendingCustomer, bool saveOnComplete = true)
        {
            Customer customer;
            if (await customerStore.ExistsFromCommonId(pendingCustomer.CommonId, pendingCustomer.EnvironmentId))
            {
                customer = await customerStore.ReadFromCommonId(pendingCustomer.CommonId, pendingCustomer.EnvironmentId, _ => _.IntegratedSystemMaps);
                logger.LogInformation($"Updating user {customer.Id}");
                if (pendingCustomer.OverwriteExisting)
                {
                    if (!string.IsNullOrEmpty(pendingCustomer.Name))
                    {
                        customer.Name = pendingCustomer.Name;
                    }
                    if (!string.IsNullOrEmpty(pendingCustomer.Email))
                    {
                        customer.Email = pendingCustomer.Email;
                    }
                    if (pendingCustomer.Properties != null && pendingCustomer.Properties.Keys.Any())
                    {
                        customer = await MergeUpdateProperties(customer, pendingCustomer.Properties, saveOnComplete: false);
                    }
                }
            }
            else
            {
                customer = await customerStore.Create(pendingCustomer.ToCoreRepresentation());
                logger.LogInformation($"Created user {customer.Id}");
            }

            if (pendingCustomer.IntegratedSystemReference != null &&
                !customer.IntegratedSystemMaps.Any(_ => _.IntegratedSystemId == pendingCustomer.IntegratedSystemReference.IntegratedSystemId))
            {
                logger.LogInformation($"Connecting user to integrated system: {pendingCustomer.IntegratedSystemReference.IntegratedSystemId}");
                var integratedSystem = await integratedSystemStore.Read(pendingCustomer.IntegratedSystemReference.IntegratedSystemId);
                await trackedUserSystemMapStore.Create(
                    new TrackedUserSystemMap(pendingCustomer.IntegratedSystemReference.UserId, integratedSystem, customer));
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

        public async Task<IEnumerable<Customer>> CreateOrUpdate(
           IEnumerable<PendingCustomer> pendingCustomers, bool saveOnComplete = true)
        {
            // check for incoming duplicates
            var numDistinct = pendingCustomers.Select(_ => _.CommonId).Distinct().Count();
            if (numDistinct < pendingCustomers.Count())
            {
                throw new BadRequestException("All CommonUserId must be unique when batch creating or updating.");
            }
            var customers = new List<Customer>();
            foreach (var pendingCustomer in pendingCustomers)
            {
                // var user = await CreateOrUpdate(u.CustomerId, u.Name, u.Email, u.Properties, u.IntegratedSystemId, u.IntegratedSystemUserId, false);
                var customer = await CreateOrUpdate(pendingCustomer, false);
                customers.Add(customer);
            }

            await storageContext.SaveChanges();
            return customers;
        }
    }
}