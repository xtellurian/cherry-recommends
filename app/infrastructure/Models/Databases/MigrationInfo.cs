namespace SignalBox.Infrastructure.Models.Databases
{
    public class MigrationInfo
    {
        public string Name { get; set; }
        public bool WillBeApplied { get; set; }

        protected MigrationInfo() { }

        public MigrationInfo(string name, bool willBeApplied)
        {
            this.Name = name;
            this.WillBeApplied = willBeApplied;
        }
    }
}