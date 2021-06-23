namespace SignalBox.Core
{
#nullable enable
    public struct CreateCommonEntityModel
    {
        public CreateCommonEntityModel(string commonId, string? name)
        {
            CommonId = commonId;
            Name = name;
        }

        public string CommonId { get; set; }
        public string? Name { get; set; }
    }
}