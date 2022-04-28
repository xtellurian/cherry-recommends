using System.Text.Json.Serialization;
using SignalBox.Core.Integrations;

namespace SignalBox.Core
{
    public class IntegratedSystemCredential : Entity
    {
        protected IntegratedSystemCredential()
        { }

        public string Key { get; set; }
        public IntegratedSystemTypes SystemType { get; set; }
        [JsonIgnore]
        public string Credentials { get; set; }
        [JsonIgnore]
        public string Config { get; set; }

        public void SetCredentials<T>(T value) where T : IIntegratedSystemCredentials
        {
            Credentials = base.Serialize(value);
        }

        public T GetCredentials<T>() where T : class, IIntegratedSystemCredentials
        {
            if (Credentials == null)
            {
                return default(T);
            }
            return base.Deserialize<T>(Credentials);
        }

        public void ClearCredentials()
        {
            Credentials = string.Empty;
        }

        public void SetConfig<T>(T value) where T : IIntegratedSystemConfig
        {
            Config = base.Serialize(value);
        }

        public T GetConfig<T>() where T : class, IIntegratedSystemConfig
        {
            if (Config == null)
            {
                return default(T);
            }
            return base.Deserialize<T>(Config);
        }

        public void ClearConfig()
        {
            Config = string.Empty;
        }
    }
}