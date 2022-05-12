using System;

namespace SignalBox.Web.Dto
{
    public class RecommenderSettingsDto : DtoBase
    {
        public bool? Enabled { get; set; }
        public bool? ThrowOnBadInput { get; set; }
        public bool? RequireConsumptionEvent { get; set; }
        public System.TimeSpan? RecommendationCacheTime { get; set; }
        /// <summary>
        /// Date after which no more recommendations will be produced
        /// </summary>
        public DateTimeOffset? ExpiryDate { get; set; }
    }
}