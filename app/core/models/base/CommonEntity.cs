namespace SignalBox.Core
{
    public abstract class CommonEntity : EnvironmentScopedEntity
    {
        protected virtual int CommonIdMinLength => 3;
        protected CommonEntity()
        { }
#nullable enable
        protected CommonEntity(string commonId, string? name)
        {
            Validate(commonId);
            CommonId = commonId;
            Name = name;
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
                throw new CommonIdException(commonId, "Common Id must not be null");
            }
            else if (commonId.Length < CommonIdMinLength)
            {
                throw new CommonIdException(commonId, $"Common Id must not be at least {CommonIdMinLength} characters");
            }
            else if (!commonId.ContainsOnlyAlphaNumeric('-', '_'))
            {
                throw new CommonIdException(commonId, $"Common Id must only contain alpha-numeric, underscore, or hyphen");
            }
        }

        public string? Name { get; set; }
        public string CommonId { get; set; }
        public DynamicPropertyDictionary? Properties { get; set; } = new DynamicPropertyDictionary(); // not empty
    }
}