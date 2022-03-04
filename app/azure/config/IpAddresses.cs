using System.Collections.Generic;

namespace SignalBox.Azure
{
    public static class IpAddresses
    {
        public static Dictionary<string, string> SqlServerWhitelist = new Dictionary<string, string>
        {
            { "bourkeStreet", "27.32.26.166" },
            { "coffeeHouse", "149.167.128.243" },
            { "retool1", "52.177.12.28" },
            { "retool2", "52.177.118.220" },
            { "retool3", "52.175.251.223" },
        };
    }
}