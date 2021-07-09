using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core;
using SignalBox.Core.Recommenders;

namespace SignalBox.Infrastructure
{
    public class RandomParameterSetRecommender : IRecommenderModelClient<ParameterSetRecommenderModelInputV1, ParameterSetRecommenderModelOutputV1>
    {
        public Task<ParameterSetRecommenderModelOutputV1> Invoke(IRecommender recommender, string version, ParameterSetRecommenderModelInputV1 input)
        {
            // model should be null

            var parameterSetRecommender = (ParameterSetRecommender)recommender;
            var random = new Random();

            var output = new ParameterSetRecommenderModelOutputV1
            {
                RecommendedParameters = new Dictionary<string, object>()
            };

            foreach (var p in parameterSetRecommender.Parameters)
            {
                var bounds = parameterSetRecommender.ParameterBounds.FirstOrDefault(_ => _.CommonId == p.CommonId);
                if (p.DefaultValue != null)
                {
                    output.RecommendedParameters[p.CommonId] = p.DefaultValue.Value;
                }
                else if (p.ParameterType == ParameterTypes.Numerical)
                {
                    output.RecommendedParameters[p.CommonId] = random.Next((int)(bounds?.NumericBounds?.Min ?? 0), (int)(bounds?.NumericBounds?.Max ?? 100));
                }
                else if (p.ParameterType == ParameterTypes.Categorical)
                {
                    output.RecommendedParameters[p.CommonId] = random.Next(10) % 2 == 0 ? "Hotdog" : "Not Hotdog";
                }
            }

            return Task.FromResult(output);
        }
    }
}