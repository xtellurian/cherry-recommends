using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Storage.Queues; // Namespace for Queue storage types
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Options;
using SignalBox.Core;

namespace SignalBox.Infrastructure.Queues
{
    public abstract class AzureQueueStoreBase<T> : IQueueStore<T> where T : IQueueMessage
    {
        private QueueClient _queueClient;
        private QueueClient queueClient
        {
            get
            {
                _queueClient ??= new QueueClient(options.Value.ConnectionString, queueName);
                return _queueClient;
            }
        }
        private JsonSerializerOptions serializerOptions = new JsonSerializerOptions();
        private readonly IOptions<AzureQueueConfig> options;
        private readonly string queueName;

        public AzureQueueStoreBase(IOptions<AzureQueueConfig> options, string queueName)
        {
            this.options = options;
            this.queueName = queueName;
        }

        private IDictionary<string, string> GetProperties(QueueMessage qm)
        {
            return new Dictionary<string, string>
            {
                { "MessageId" , qm.MessageId},
                { "InsertedOn" , qm.InsertedOn.ToString()},
                { "ExpiresOn" , qm.ExpiresOn.ToString()},
                { "PopReceipt" , qm.PopReceipt}
            };
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public Task<bool> IsWriteEnabled()
        {
            return Task.FromResult(
                !string.IsNullOrEmpty(this.options.Value?.ConnectionString)
                && this.options.Value.EnableWriteQueue);
        }

        public Task<bool> IsReadEnabled()
        {
            return Task.FromResult(
                !string.IsNullOrEmpty(this.options.Value?.ConnectionString)
                && this.options.Value.EnableWriteQueue);
        }

        public async Task Enqueue(T message)
        {
            try
            {
                var stringified = JsonSerializer.Serialize(message, serializerOptions);
                var encoded = Base64Encode(stringified);
                var result = await queueClient.SendMessageAsync(encoded);
            }
            catch (System.Exception ex)
            {
                throw new QueueStorageException($"Failed to Enqueue onto queue ${this.queueName}", ex);
            }
        }

        public async Task<IEnumerable<DequeuedMessage<T>>> StartDequeue()
        {
            try
            {
                var retrieved = await queueClient.ReceiveMessagesAsync();
                return retrieved.Value
                    .Select(_ =>
                        new DequeuedMessage<T>(_.Body.ToObjectFromJson<T>(serializerOptions), GetProperties(_)));

            }
            catch (System.Exception ex)
            {
                throw new QueueStorageException($"Failed to StartDequeue off queue ${this.queueName}", ex);
            }
        }

        public async Task CompleteDequeue(IEnumerable<DequeuedMessage<T>> dequeuedMessages)
        {
            try
            {
                foreach (var m in dequeuedMessages)
                {
                    await queueClient.DeleteMessageAsync(m.Properties["MessageId"], m.Properties["PopReceipt"]);
                }
            }
            catch (System.Exception ex)
            {
                throw new QueueStorageException($"Failed to CompleteDequeue off queue ${this.queueName}", ex);
            }
        }


    }
}