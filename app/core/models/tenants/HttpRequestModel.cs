using System.Collections.Generic;

namespace SignalBox.Core
{
    public class HttpRequestModel
    {
        public HttpRequestModel(string path, IDictionary<string, string> headers, IDictionary<string, string> routeValueDictionary, IDictionary<string, string> queryDictionary)
        {
            Path = path;
            Headers = new Dictionary<string, string>(headers);
            RouteValueDictionary = routeValueDictionary;
            QueryDictionary = queryDictionary;
        }
        public string Path { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public IDictionary<string, string> RouteValueDictionary { get; set; }
        public IDictionary<string, string> QueryDictionary { get; set; }
    }
}