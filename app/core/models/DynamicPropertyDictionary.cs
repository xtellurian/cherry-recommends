using System.Collections.Generic;

namespace SignalBox.Core
{
    public class DynamicPropertyDictionary : Dictionary<string, object>
    {

        public DynamicPropertyDictionary()
        {
        }

        public DynamicPropertyDictionary(IDictionary<string, object> dictionary) : base(dictionary ?? new Dictionary<string,object>())
        {
        }
    }
}