using System.Collections.Generic;

namespace SignalBox.Azure
{
    public static class IpAddresses
    {
        public static Dictionary<string, string> SqlServerWhitelist = new Dictionary<string, string>
        {
            { "bourkeStreet", "27.32.26.166" },
            { "coffeeHouse", "149.167.128.243" }
        };
    }
}