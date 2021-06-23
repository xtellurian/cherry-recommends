namespace SignalBox.Core
{
    public abstract class CommonEntity : NamedEntity
    {
        protected CommonEntity()
        { }
        protected CommonEntity(string commonId, string name) : base(name)
        {
            CommonId = commonId;
        }

        public string CommonId { get; set; }
    }
}