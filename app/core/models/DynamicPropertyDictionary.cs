using System.Collections.Generic;

namespace SignalBox.Core
{
    public class DynamicPropertyDictionary : Dictionary<string, object>
    {

        public DynamicPropertyDictionary()
        {
        }

        public DynamicPropertyDictionary(IDictionary<string, object> dictionary) : base(dictionary ?? new Dictionary<string, object>())
        {
        }

        public void Validate()
        {
            if (this.Keys.Count > 31)
            {
                throw new BadRequestException("You cannot have more than 32 properties");
            }

            foreach (var key in this.Keys)
            {
                var value = this[key];

                if (value is System.Text.Json.JsonElement jsonElement)
                {
                    if (jsonElement.ValueKind == System.Text.Json.JsonValueKind.String || jsonElement.ValueKind == System.Text.Json.JsonValueKind.Number)
                    {
                        continue;
                    }
                    else
                    {
                        throw new BadRequestException($"The value {value} has invalid json value kind {jsonElement.ValueKind }. It cannot be used in a property.");
                    }
                }
                if (value is null)
                {
                    continue;
                }
                else if (value is string)
                {
                    continue;
                }
                else if (value is int)
                {
                    continue;
                }
                else if (value is double)
                {
                    continue;
                }
                else if (value is double)
                {
                    continue;
                }
                else
                {
                    throw new BadRequestException($"The value {value} has invalid type {value.GetType()}. It cannot be used in a property.");
                }
            }
        }
    }
}