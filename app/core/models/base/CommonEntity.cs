namespace SignalBox.Core
{
    public abstract class CommonEntity : EnvironmentScopedEntity
    {
        protected virtual int CommonIdMinLength => 3;
        protected CommonEntity()
        { }
#nullable enable
        protected CommonEntity(string commonId, string? name) : base(name)
        {
            Validate(commonId);
            CommonId = commonId.ToLower();
        }

        protected CommonEntity(string commonId, string? name, DynamicPropertyDictionary? properties) : this(commonId, name)
        {
            Validate(commonId);
            this.Properties = properties ?? new DynamicPropertyDictionary();
        }

        private void Validate(string commonId)
        {
            if (string.IsNullOrEmpty(commonId))
            {
                throw new CommonIdException("Common Id must not be null");
            }
            else if (commonId.Length < CommonIdMinLength)
            {
                throw new CommonIdException($"Common Id must not be at least {CommonIdMinLength} characters");
            }
        }

        public string CommonId { get; set; }
        public DynamicPropertyDictionary? Properties { get; set; } = new DynamicPropertyDictionary(); // not empty
    }
}