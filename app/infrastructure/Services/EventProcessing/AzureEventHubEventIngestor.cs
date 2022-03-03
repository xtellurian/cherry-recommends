using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Options;
using SignalBox.Core;
using SignalBox.Core.Serialization;
using SignalBox.Core.Workflows;
using SignalBox.Infrastructure.Models;

namespace SignalBox.Infrastructure.Services
{
#nullable enable
    public class AzureEventHubEventIngestor : IEventIngestor, IAsyncDisposable
    {
        private readonly EventHubProducerClient? producerClient;

        public AzureEventHubEventIngestor(IOptions<EventhubConfig> eventhubConfig)
        {
            if (eventhubConfig.Value.ConnectionString != null)
            {
                this.producerClient = new EventHubProducerClient(eventhubConfig.Value.ConnectionString, eventhubConfig.Value.EventhubName);
            }

        }

        public bool CanIngest => producerClient != null;

        public async ValueTask DisposeAsync()
        {
            if (producerClient != null)
            {
                await producerClient.DisposeAsync();
                GC.SuppressFinalize(this);
            }
        }

        public async Task Ingest(IEnumerable<CustomerEventInput> inputs)
        {
            if (producerClient == null)
            {
                throw new ConfigurationException("producerClient is not initialised");
            }
            // Create a batch of events 
            using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();
            foreach (var i in inputs)
            {
                var ed = new EventData(Serializer.Serialize(i));
                if (!eventBatch.TryAdd(ed))
                {
                    // if it is too large for the batch
                    throw new BadRequestException($"Event {i.EventId} is too large for the batch and cannot be sent.");
                }
            }

            // Use the producer client to send the batch of events to the event hub
            await producerClient.SendAsync(eventBatch);
        }
    }
}