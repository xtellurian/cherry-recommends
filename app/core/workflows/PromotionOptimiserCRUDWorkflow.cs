using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core.Optimisers;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core.Workflows
{
#nullable enable
    public class PromotionOptimiserCRUDWorkflow : IWorkflow, IPromotionOptimiserCRUDWorkflow
    {
        private readonly IPromotionOptimiserStore store;
        private readonly IItemsRecommenderStore recommenderStore;

        public PromotionOptimiserCRUDWorkflow(IPromotionOptimiserStore store, IItemsRecommenderStore recommenderStore)
        {
            this.store = store;
            this.recommenderStore = recommenderStore;
        }


        public async Task<PromotionOptimiser> Create(ItemsRecommender recommender)
        {
            if (recommender.BaselineItemId is null)
            {
                throw new BadRequestException("Recommender has null baseline");
            }
            await recommenderStore.LoadMany(recommender, _ => _.Items); // ensure the items are loaded.
            var optimiser = await store.Create(new PromotionOptimiser(recommender));
            recommender.Optimiser = optimiser;
            optimiser.InitialiseWeights(recommender);
            await store.Context.SaveChanges();
            return optimiser;
        }

        /// <summary>
        /// Read the Optimiser for a recommender from the database.
        /// This will automatically update any weights as necessary, if the items have changed from the recommender.
        /// </summary>
        /// <param name="recommenderId"></param>
        /// <param name="useInternalId"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<PromotionOptimiser> Read(string recommenderId, bool? useInternalId = null)
        {
            var recommender = await recommenderStore.GetEntity(recommenderId, useInternalId);
            await recommenderStore.Load(recommender, _ => _.Optimiser);
            if (recommender.Optimiser == null)
            {
                if (recommender.UseOptimiser)
                {
                    return await Create(recommender);
                }
                else
                {
                    throw new BadRequestException("Cannot auto-create optimiser when Recommender has UseOptimiser=false");
                }
            }

            if (recommender.UseOptimiser)
            {
                // only update if using optimiser
                recommender.Optimiser.UpdateWeights(recommender);
                await recommenderStore.Context.SaveChanges();
            }

            return recommender.Optimiser;
        }

        public async Task<PromotionOptimiser> UpdateWeight(string recommenderId, long weightId, double weight, bool? useInternalId = null)
        {
            if (weight < 0)
            {
                throw new BadRequestException("Weight must not be negative");
            }
            var optimiser = await Read(recommenderId, useInternalId);
            if (optimiser.Weights.Any(_ => _.Id == weightId))
            {
                var w = optimiser.Weights.First(_ => _.Id == weightId);
                w.Weight = weight;
            }
            else
            {
                throw new BadRequestException($"Weight with ID {weightId} doesn't exist on Optimiser {optimiser.Id}");
            }
            optimiser.Weights = optimiser.Weights.Normalize().ToList();
            await store.Context.SaveChanges();
            return optimiser;
        }

        public async Task<bool> Delete(long id)
        {
            var success = await store.Remove(id);
            await store.Context.SaveChanges();
            return success;
        }

        public async Task<PromotionOptimiser> UpdateAllWeights(string recommenderId, IEnumerable<IWeighted> weights, bool? useInternalId = null)
        {
            var optimiser = await Read(recommenderId, useInternalId);
            optimiser.UpdateWeights(optimiser.Recommender);
            var optimiserWeights = optimiser.Weights.ToList();
            foreach (var w in weights)
            {
                if (optimiserWeights.Any(_ => _.Id == w.Id))
                {
                    optimiserWeights.First(_ => _.Id == w.Id).Weight = w.Weight;
                }
                else
                {
                    throw new BadRequestException($"Optimiser {optimiser.Id} does not contain weight id {w.Id}");
                }
            }

            optimiser.Weights = optimiserWeights.Normalize().ToList();
            await store.Context.SaveChanges();
            return optimiser;
        }
    }
}