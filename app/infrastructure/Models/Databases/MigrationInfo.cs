namespace SignalBox.Infrastructure.Models.Databases
{
    public class MigrationInfo
    {
        public string Name { get; set; }
        public bool IsApplied { get; set; }

        protected MigrationInfo() { }

        public MigrationInfo(string name, bool isApplied)
        {
            this.Name = name;
            this.IsApplied = isApplied;
        }
    }
}