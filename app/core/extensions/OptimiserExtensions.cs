using System;
using System.Collections.Generic;
using System.Linq;
using SignalBox.Core.Optimisers;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core
{
#nullable enable
    public static class OptimiserExtensions
    {
        /// <summary>
        /// Sets the initial weights on the optimiser.
        /// </summary>
        /// <param name="optimiser"></param>
        /// <param name="recommender"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static PromotionOptimiser InitialiseWeights(this PromotionOptimiser optimiser, PromotionsCampaign recommender, long? segmentId = null)
        {
            if (recommender.Items.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(recommender.Items));
            }

            var initialWeight = 1 / (double)recommender.Items.Count();
            var weights = recommender.Items.Select(i => new PromotionOptimiserWeight(i, optimiser, initialWeight, segmentId)).ToList(); // ! needs to be a list.
            if (recommender.BaselineItemId != null)
            {
                // set the baseline item 1 to begin with, and we'll renormalise straight away.
                var baselinePromoWeight = weights.First(_ => _.PromotionId == recommender.BaselineItemId);
                baselinePromoWeight.Weight = 1;
            }

            // create list for the original weights if any
            var allWeights = optimiser.Weights.ToList();
            // add the new weights 
            allWeights.AddRange(weights.Normalize());
            optimiser.Weights = allWeights;
            return optimiser;
        }

        /// <summary>
        /// Updates weights by removing and adding promotions.
        /// </summary>
        /// <param name="optimiser"></param>
        /// <param name="recommender"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static PromotionOptimiser UpdateWeights(this PromotionOptimiser optimiser, PromotionsCampaign recommender)
        {
            if (recommender.Items.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(recommender.Items));
            }
            var initialWeight = 1 / (double)recommender.Items.Count();
            // remove any weights that aren't in the recommender any more
            var promoIdsInRecommender = recommender.Items.Select(_ => _.Id);
            var existingPromotionIds = optimiser.Weights.Select(_ => _.PromotionId).Distinct();
            // keep weights that are in the recommender.
            optimiser.Weights = optimiser.Weights.Where(_ => promoIdsInRecommender.Contains(_.PromotionId)).ToList();

            // get all segment Ids
            var segmentIds = optimiser.Weights
               .Select(_ => _.SegmentId).Distinct().ToList();

            // add any new promotions to all segment weights
            foreach (var segmentId in segmentIds)
            {
                // calculate new weights
                var newWeights = recommender.Items
                    .Where(_ => !existingPromotionIds.Contains(_.Id))
                    .Select(i => new PromotionOptimiserWeight(i, optimiser, initialWeight, segmentId));

                foreach (var n in newWeights)
                {
                    optimiser.Weights.Add(n);
                }

                if (optimiser.Weights.Any(_ => _.SegmentId == segmentId))
                {
                    // get weights associated with the segment Id then normalize
                    var distributionWeights = optimiser.Weights.Where(_ => _.SegmentId == segmentId).ToList();
                    distributionWeights.Normalize();
                }
            }
            return optimiser;
        }

        /// <summary>
        /// Normalizes the weights. Returns the same as the input.
        /// This normalises everything in the list, regardless of whether it's in the same segment.
        /// </summary>
        /// <param name="weights">A collection of weights to be normalised. </param>
        public static IEnumerable<T> Normalize<T>(this IEnumerable<T> weights) where T : IWeighted
        {
            var sum = weights.Sum(_ => _.Weight);
            foreach (var w in weights)
            {
                w.Weight /= sum;
            }
            return weights;
        }

        /// <summary>
        /// Removes weights on the optimiser.
        /// </summary>
        /// <param name="optimiser"></param>
        /// <param name="campaign"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static PromotionOptimiser RemoveWeights(this PromotionOptimiser optimiser, PromotionsCampaign campaign, long segmentId)
        {
            if (campaign.Items.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(campaign.Items));
            }

            var allWeights = optimiser.Weights.ToList();
            allWeights.RemoveAll(_ => _.SegmentId == segmentId);

            optimiser.Weights = allWeights;
            return optimiser;
        }
    }
}