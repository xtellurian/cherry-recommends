using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SignalBox.Client
{
    public class SignalBoxClient
    {
        private readonly Authenticator authenticator;
        public readonly Connection connection;
        private HttpClient httpClient;
        private string baseUrl;

        public SignalBoxClient(Authenticator authenticator, IOptions<Connection> connection, IHttpClientFactory factory)
        : this(authenticator, connection.Value, factory.CreateClient())
        { }
        protected SignalBoxClient(Authenticator authenticator, Connection connection, HttpClient httpClient = null)
        {
            if (httpClient != null)
            {
                this.httpClient = httpClient;
            }
            else
            {
                this.httpClient = new HttpClient();
            }

            this.httpClient.BaseAddress = new System.Uri($"https://{connection.Host}");
            this.authenticator = authenticator;
            this.connection = connection;
            this.baseUrl = this.httpClient.BaseAddress.ToString();
        }
        public async Task<JavascriptConfiguration> GetJavascriptConfiguration()
        {
            var auth = await authenticator.Authenticate();
            return new JavascriptConfiguration(this.baseUrl, auth);
        }
    }
}