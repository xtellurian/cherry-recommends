namespace SignalBox.Core
{
    public struct CreateParameterModel
    {
        public CreateParameterModel(CreateCommonEntityModel commonEntityModel, string parameterType, string description = null)
        {
            Common = commonEntityModel;
            Description = description;
            ParameterType = parameterType;
        }

        public CreateCommonEntityModel Common { get; set; }
        public string Description { get; set; }
        public string ParameterType { get; set; }
    }
}