namespace SignalBox.Core
{
    public class AzureMLClassifierOutput : IModelOutput
    {
        public AzureMLClassifierOutput(string result)
        {
            Result = result;
        }

        public string Result { get; set; }
        public long? CorrelatorId { get; set; }
    }
}