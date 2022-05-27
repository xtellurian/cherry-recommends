using System.ComponentModel.DataAnnotations;

namespace SignalBox.Core.Campaigns
{
    public abstract class TriggerBase
    {
        protected TriggerBase() { }

        protected TriggerBase(string name)
        {
            this.Name = name;
        }

        [Required]
        public string Name { get; set; }
    }
}