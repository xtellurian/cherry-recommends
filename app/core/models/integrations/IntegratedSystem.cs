using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Integrations;
using SignalBox.Core.OAuth;

namespace SignalBox.Core
{
    public class IntegratedSystem : CommonEntity, IHierarchyBase
    {
        protected IntegratedSystem()
        { }
        public IntegratedSystem(string commonId, string name, IntegratedSystemTypes systemType) : base(commonId, name)
        {
            SystemType = systemType;
            IntegrationStatus = IntegrationStatuses.NotConfigured;
        }

        public void SetCache<T>(T value) where T : IIntegratedSystemCache
        {
            this.Cache = base.Serialize(value);
        }

        public T GetCache<T>() where T : IIntegratedSystemCache
        {
            if (this.Cache == null)
            {
                return default(T);
            }
            return base.Deserialize<T>(this.Cache);
        }

        public IntegratedSystemTypes SystemType { get; set; }

        public IntegrationStatuses IntegrationStatus { get; set; }

        [JsonIgnore]
        public ICollection<WebhookReceiver> WebhookReceivers { get; set; }
        public DateTimeOffset? TokenResponseUpdated { get; set; }
        [JsonIgnore]
        public TokenResponse TokenResponse { get; set; }
        [JsonIgnore]
        public string ApiKey { get; set; }

        [JsonIgnore]
        public DateTimeOffset? CacheLastRefreshed { get; set; }
        [JsonIgnore]
        public string Cache { get; set; }
        public string Discriminator { get; set; }
    }
}