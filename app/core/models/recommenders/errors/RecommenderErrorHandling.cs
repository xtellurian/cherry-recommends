namespace SignalBox.Core.Recommenders
{
    public class RecommenderErrorHandling : RecommenderSettings
    {
        public RecommenderErrorHandling() { }
        public RecommenderErrorHandling(RecommenderSettings other)
        {
            this.ThrowOnBadInput = other.ThrowOnBadInput;
            this.RequireConsumptionEvent = other.RequireConsumptionEvent;
        }
    }
}