namespace SignalBox.Core
{
    public abstract class NamedEntity : Entity
    {
        protected NamedEntity()
        { }

        protected NamedEntity(string name)
        {
            Name = name;
        }
        public string Name { get; set; } // settable by the creator
    }
}