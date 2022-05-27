using System;
using System.Collections.Generic;
using System.Linq;
using SignalBox.Core.Optimisers;
using SignalBox.Core.Campaigns;

namespace SignalBox.Core
{
#nullable enable
    // TODO: make this class Segment aware, i.e. deal with different segment distributions
    public static class OptimiserExtensions
    {
        /// <summary>
        /// Sets the initial weights on the optimiser.
        /// </summary>
        /// <param name="optimiser"></param>
        /// <param name="recommender"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static PromotionOptimiser InitialiseWeights(this PromotionOptimiser optimiser, PromotionsCampaign recommender)
        {
            if (recommender.Items.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(recommender.Items));
            }

            var initialWeight = 1 / (double)recommender.Items.Count();
            var weights = recommender.Items.Select(i => new PromotionOptimiserWeight(i, optimiser, initialWeight)).ToList(); // ! needs to be a list.
            if (recommender.BaselineItemId != null)
            {
                // set the baseline item 1 to begin with, and we'll renormalise straight away.
                var baselinePromoWeight = weights.First(_ => _.PromotionId == recommender.BaselineItemId);
                baselinePromoWeight.Weight = 1;
            }

            optimiser.Weights = weights.Normalize().ToList();
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
            var existingPromotionIds = optimiser.Weights.Select(_ => _.PromotionId);
            // keep weights that are in the recommender.
            optimiser.Weights = optimiser.Weights.Where(_ => promoIdsInRecommender.Contains(_.PromotionId)).ToList();
            // calculate new weights
            var newWeights = recommender.Items
                .Where(_ => !existingPromotionIds.Contains(_.Id))
                .Select(i => new PromotionOptimiserWeight(i, optimiser, initialWeight));

            foreach (var n in newWeights)
            {
                optimiser.Weights.Add(n);
            }

            optimiser.Weights = optimiser.Weights.Normalize().ToList();

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
    }
}