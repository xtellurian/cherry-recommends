namespace SignalBox.Core.Internal
{
#nullable enable
    public class UserMetadata
    {
        public void EnsureInitialised()
        {
            gettingStartedChecklist ??= new GettingStartedChecklist();
            gettingStartedChecklist.EnsureInitialised();
        }
        // this is camelCase because auth0 dynamic obj doesn't support controlling serialization. 
        public GettingStartedChecklist? gettingStartedChecklist { get; set; } = new GettingStartedChecklist();
    }
}