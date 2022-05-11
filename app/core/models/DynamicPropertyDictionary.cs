using System.Collections.Generic;
using System.Linq;

namespace SignalBox.Core
{
#nullable enable
    public class DynamicPropertyDictionary : Dictionary<string, object>
    {
        public void Merge(IDictionary<string, object>? other)
        {
            if (other == null)
            {
                return;
            }
            foreach (var key in other.Keys)
            {
                this[key] = other[key];
            }
        }

        public void Merge(IDictionary<string, string> other)
        {
            if (other == null)
            {
                return;
            }
            foreach (var key in other.Keys)
            {
                this[key] = other[key];
            }
        }

        public void PrefixAllKeys(string? prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                return;
            }
            var oldKeys = this.Keys.ToList();
            foreach (var k in oldKeys)
            {
                if (!k.StartsWith(prefix))
                {
                    var value = this[k];
                    this.Remove(k);
                    this.Add($"{prefix}{k}", value);
                }
            }
        }

        public DynamicPropertyDictionary()
        {
        }

        public DynamicPropertyDictionary(IDictionary<string, object>? dictionary) : base(dictionary ?? new Dictionary<string, object>())
        {
        }

        public DynamicPropertyDictionary(IDictionary<string, string> dictionary)
        : base(dictionary?.ToDictionary(pair => pair.Key, pair => (object)pair.Value) ?? new Dictionary<string, object>())
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
                        throw new BadRequestException($"The value {value} has invalid json value kind {jsonElement.ValueKind}. It cannot be used in a property.");
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