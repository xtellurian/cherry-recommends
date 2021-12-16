using System;
using SignalBox.Core;

namespace SignalBox.Web.Dto
{
    public class ApiKeyDto
    {
        public ApiKeyDto(HashedApiKey hashedApiKey)
        {
            Id = hashedApiKey.Id;
            Name = hashedApiKey.Name;
            LastExchanged = hashedApiKey.LastExchanged;
            TotalExchanges = hashedApiKey.TotalExchanges;
            this.ApiKeyType = hashedApiKey.ApiKeyType;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset? LastExchanged { get; set; }
        public int TotalExchanges { get; set; }
        public ApiKeyTypes? ApiKeyType { get; private set; }
    }
}