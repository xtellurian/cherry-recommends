using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public class Parameter : CommonEntity
    {
        public Parameter()
        { }
        public Parameter(CreateCommonEntityModel model, ParameterTypes parameterType, string description = null)
         : base(model.CommonId, model.Name)
        {
            this.ParameterType = parameterType;
            this.Description = description;
        }

        public ParameterTypes ParameterType { get; set; }
        public DefaultParameterValue DefaultValue { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public ICollection<ParameterSetRecommender> ParameterSetRecommenders { get; set; }

        public T SetDefaultValue<T>(T val)
        {
            DefaultValue = new DefaultParameterValue(ParameterType, val);
            return DefaultValue.GetValue<T>();
        }
    }
}