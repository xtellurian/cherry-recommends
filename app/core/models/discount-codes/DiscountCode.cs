using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SignalBox.Core.Recommendations;

namespace SignalBox.Core
{
    public class DiscountCode : EnvironmentScopedEntity
    {
        protected DiscountCode()
        { }

        public DiscountCode(RecommendableItem promotion, string code, DateTimeOffset? startsAt = null, DateTimeOffset? endsAt = null)
        {
            PromotionId = promotion.Id;
            Promotion = promotion;
            Code = code;
            StartsAt = startsAt;
            EndsAt = endsAt;
        }

        public string Code { get; set; }
        public DateTimeOffset? StartsAt { get; set; }
        public DateTimeOffset? EndsAt { get; set; }
        public long PromotionId { get; set; }

        [JsonIgnore]
        public RecommendableItem Promotion { get; set; }
        [JsonIgnore]
        public ICollection<ItemsRecommendation> Recommendations { get; set; }
        public ICollection<IntegratedSystem> GeneratedAt { get; set; } = new List<IntegratedSystem>();

        public bool WasCreatedOnDate(DateTimeOffset createdDate)
        {
            return Created.TruncateToDayStart() == createdDate.TruncateToDayStart();
        }

        public bool IsActiveByDate(DateTimeOffset dateTime)
        {
            return StartsAt.HasValue &&
                (!EndsAt.HasValue || (EndsAt.HasValue && EndsAt.Value >= dateTime));
        }

        public static string GenerateCode(string commonId, int codeLength = 8)
        {
            string alphaNumeric = commonId.ToAlphaNumeric();
            int partALength = alphaNumeric.Length > 5 ? 5 : alphaNumeric.Length;
            int partBLength = codeLength - partALength;
            string partA = alphaNumeric.Substring(0, partALength);
            string partB = StringExtensions.RandomString(partBLength);

            string code = $"{partA}{partB}".ToUpper(); // string length is equal to codeLength

            return code;
        }
    }
}