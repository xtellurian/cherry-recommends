using System;
using System.Collections.Generic;
using System.Linq;
using SignalBox.Core;
using SignalBox.Core.Optimisers;
using SignalBox.Core.Campaigns;
using Xunit;

namespace SignalBox.Test.Optimisers
{
    public class NormalizationTests
    {
        private const int precision = 10;
        private static string RndStr => System.Guid.NewGuid().ToString();

        [Fact]
        public void EmptyRecommenderPromotions_Throws()
        {
            // arrange
            var recommender = new PromotionsCampaign(RndStr,
                                                   RndStr,
                                                   baselineItem: null,
                                                   promotions: Enumerable.Empty<RecommendableItem>().ToList(),
                                                   null,
                                                   null,
                                                   null,
                                                   1);
            var sut = new PromotionOptimiser(recommender);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => sut.InitialiseWeights(recommender));
        }

        [Fact]
        public void Normalise_HappyPath()
        {
            // arrange
            var nPromos = 97; // 97 is prime
            var promotions = new List<RecommendableItem>();
            for (int n = 0; n < nPromos; n++)
            {
                promotions.Add(EntityFactory.Promotion().WithId());
            }
            var baselinePromotion = promotions.First();
            var recommender = new PromotionsCampaign(RndStr, RndStr, promotions.First(), promotions, null, null, null, 1);

            var sut = new PromotionOptimiser(recommender);

            // act
            var output = sut.InitialiseWeights(recommender);
            // assert
            Assert.Equal(sut, output);
            var minWeight = output.Weights.Min(_ => _.Weight);
            var maxWeight = output.Weights.Max(_ => _.Weight);
            Assert.NotEqual(maxWeight, minWeight, precision);
            var baselineWeight = output.Weights.First(_ => _.PromotionId == baselinePromotion.Id).Weight;
            Assert.Equal(maxWeight, baselineWeight, precision);
            Assert.Equal(1, sut.Weights.Sum(_ => _.Weight), precision); // 10 decimal places
        }

        private class TestWeightedItem : IWeighted
        {
            public TestWeightedItem(int id, double weight)
            {
                Id = id;
                Weight = weight;
            }
            public long Id { get; }
            public double Weight { get; set; }
        }

        [Fact]
        public void WeightedRandomSelector_Normalized_ActuallyChoosesWell()
        {
            var rnd = new Random();
            var nItems = 100;
            var selectCounts = new Dictionary<long, int>();
            var items = new List<TestWeightedItem>
            {
                new TestWeightedItem(1, 10) // this one has a high probability
            };
            selectCounts.Add(1, 0);
            for (var n = 2; n <= nItems; n++)
            {
                items.Add(new TestWeightedItem(n, 0.1 + rnd.NextDouble())); // ensure that the weights are non zero
                selectCounts.Add(n, 0);
            }
            items.Normalize();

            var sut = new WeightedRandomSelector<TestWeightedItem>(items);


            // act 10k times 
            for (var t = 0; t < 10000; t++)
            {
                var s = sut.Choose();
                selectCounts[s.Id] += 1;
            }

            // assert
            Assert.True(selectCounts.Keys.Count == nItems); // check all the items are represented
            Assert.True(selectCounts.Values.All(c => c > 0)); // assert everything was selected at least once
            var maxSelectedValue = selectCounts.Values.Max();
            Assert.True(maxSelectedValue > 1);
            var maxId = selectCounts.Where(kvp => kvp.Value == maxSelectedValue).FirstOrDefault().Key;
            Assert.Equal(1, maxId);
        }

        [Fact]
        public void PromotionOptimiser_Extension_CanUpdateWeights()
        {
            // arrange
            var nPromos = 11;
            var promotions = new List<RecommendableItem>();
            for (int n = 0; n < nPromos; n++)
            {
                promotions.Add(EntityFactory.Promotion().WithId());
            }
            var baselinePromotion = promotions.First();
            var recommender = new PromotionsCampaign(RndStr, RndStr, promotions.First(), promotions, null, null, null, 1);

            var sut = new PromotionOptimiser(recommender);
            sut = sut.InitialiseWeights(recommender);

            // then add/ remove some items and see what happens
            var itemToRemove = recommender.Items.ToList()[3];
            if (!recommender.Items.Remove(itemToRemove))
            {
                throw new Exception("Didnt remove item");
            }
            var itemToAdd = EntityFactory.Promotion().WithId();
            recommender.Items.Add(itemToAdd);

            // act
            var output = sut.UpdateWeights(recommender);

            // assert
            Assert.Equal(sut, output);
            Assert.Equal(1, sut.Weights.Sum(_ => _.Weight), 10); // 10 decimal places
            Assert.DoesNotContain(itemToRemove.CommonId, output.Weights.Select(_ => _.Promotion.CommonId));
            Assert.Contains(itemToAdd.CommonId, output.Weights.Select(_ => _.Promotion.CommonId));
            Assert.Equal(recommender.Items.Count, sut.Weights.Count); // only true for null segment list
        }
    }
}