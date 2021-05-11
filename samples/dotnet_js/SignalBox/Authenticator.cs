using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace SignalBox.Client
{
    public class Authenticator
    {
        private readonly Connection connection;
        private readonly HttpClient httpClient;

        public Authenticator(IOptions<Connection> connection, IHttpClientFactory clientFactory) : this(connection.Value, clientFactory.CreateClient())
        { }

        protected Authenticator(Connection connection, HttpClient client)
        {
            connection.Verify();
            if (client != null)
            {
                this.httpClient = client;
            }
            else
            {
                this.httpClient = new HttpClient();
            }

            this.connection = connection;
            this.httpClient.BaseAddress = new System.Uri($"https://{connection.Host}");
        }

        private DateTimeOffset lastAuthenticated;
        public AuthenticationResponse Auth { get; private set; }

        public async Task<AuthenticationResponse> Authenticate()
        {
            if (Auth == null)
            {
                await Reauthenticate();
            }
            else if (DateTime.Now > lastAuthenticated.AddSeconds(Auth.ExpiresIn))
            {
                await Reauthenticate();
            }

            return this.Auth;
        }

        private async Task Reauthenticate()
        {
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            nvc.Add(new KeyValuePair<string, string>("client_id", connection.ClientId));
            nvc.Add(new KeyValuePair<string, string>("client_secret", connection.ClientSecret));

            var result = await httpClient.PostAsync("/connect/token", new FormUrlEncodedContent(nvc));

            var body = await result.Content.ReadAsStringAsync();
            this.Auth = JsonConvert.DeserializeObject<AuthenticationResponse>(body);
            lastAuthenticated = DateTime.Now;
        }
    }
}