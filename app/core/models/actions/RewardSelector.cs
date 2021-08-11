namespace SignalBox.Core
{
    public class RewardSelector : Entity
    {
        protected RewardSelector()
        { }
        public RewardSelector(string actionName, SelectorTypes selectorType, string category = null)
        {
            this.ActionName = actionName;
            this.SelectorType = selectorType;
            this.Category = category;
        }
#nullable enable
        public string? Category { get; set; }
        public string? ActionName { get; set; }
        public SelectorTypes SelectorType { get; set; }
    }
}