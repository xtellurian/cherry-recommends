namespace SignalBox.Core
{
    public struct CreateParameterModel
    {
        #nullable enable
        public CreateParameterModel(CreateCommonEntityModel commonEntityModel, string parameterType, object? defaultValue, string? description = null)
        {
            Common = commonEntityModel;
            Description = description;
            ParameterType = parameterType;
            DefaultValue = defaultValue;
        }

        public CreateCommonEntityModel Common { get; set; }
        public string? Description { get; set; }
        public string ParameterType { get; set; }
        public object? DefaultValue { get; set; }
    }
}