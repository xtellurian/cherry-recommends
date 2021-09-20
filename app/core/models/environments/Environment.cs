namespace SignalBox.Core
{
    public class Environment : Entity
    {
        protected Environment() { }
        public Environment(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}