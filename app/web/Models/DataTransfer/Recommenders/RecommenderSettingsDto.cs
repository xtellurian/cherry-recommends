namespace SignalBox.Web.Dto
{
    public class RecommenderSettingsDto : DtoBase
    {
        public bool? Enabled { get; set; }
        public bool? ThrowOnBadInput { get; set; }
        public bool? RequireConsumptionEvent { get; set; }
        public System.TimeSpan? RecommendationCacheTime { get; set; }
    }
}