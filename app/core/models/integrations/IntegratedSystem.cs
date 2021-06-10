namespace SignalBox.Core
{
    public class IntegratedSystem : NamedEntity
    {
        public IntegratedSystem(string name, IntegratedSystemTypes systemType) : base(name)
        {
            SystemType = systemType;
        }

        public IntegratedSystemTypes SystemType { get; set; }
    }
}