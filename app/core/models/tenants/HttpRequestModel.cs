using System.Collections.Generic;

namespace SignalBox.Core
{
    public class HttpRequestModel
    {
        public HttpRequestModel( string path, IDictionary<string, string> headers)
        {
            this.Path = path;
            this.Headers = new Dictionary<string, string>(headers); 
        }
        public string Path { get; set; }
        public IDictionary<string, string> Headers { get; set; }
    }
}