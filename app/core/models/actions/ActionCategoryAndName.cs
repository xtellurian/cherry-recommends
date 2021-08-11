namespace SignalBox.Core
{
    public class ActionCategoryAndName
    {
        public ActionCategoryAndName(string category, string actionName)
        {
            Category = category;
            ActionName = actionName;
        }

        public string Category { get; set; }
        public string ActionName { get; set; }
    }
}