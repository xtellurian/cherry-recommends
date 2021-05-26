namespace SignalBox.Core
{
    public class IntegratedSystem : NamedEntity
    {
        public IntegratedSystem(string name, string systemType) : base(name)
        {
            SystemType = systemType;
        }

        public string SystemType { get; set; }
    }
}