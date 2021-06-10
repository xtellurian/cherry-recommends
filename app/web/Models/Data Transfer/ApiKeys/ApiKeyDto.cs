using System;

namespace SignalBox.Web.Dto
{
    public class ApiKeyDto
    {
        public ApiKeyDto(long id, string name, DateTimeOffset? lastExchanged, int totalExchanges)
        {
            Id = id;
            Name = name;
            LastExchanged = lastExchanged;
            TotalExchanges = totalExchanges;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset? LastExchanged { get; set; }
        public int TotalExchanges { get; set; }
    }
}