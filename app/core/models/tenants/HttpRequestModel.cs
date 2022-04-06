using System.Collections.Generic;

namespace SignalBox.Core
{
    public class HttpRequestModel
    {
        public HttpRequestModel(string path, IDictionary<string, string> headers, IDictionary<string, string> routeValueDictionary)
        {
            Path = path;
            RouteValueDictionary = routeValueDictionary;
            Headers = new Dictionary<string, string>(headers);
        }
        public string Path { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public IDictionary<string, string> RouteValueDictionary { get; set; }
    }
}