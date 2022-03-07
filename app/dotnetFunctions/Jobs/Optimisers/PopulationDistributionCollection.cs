using System.Collections.Generic;
//using System.Text.Json.Serialization;
using SignalBox.Core;

namespace SignalBox.Functions
{
    public class PopulationDistributionCollection
    {
        private const int ConstantPopulationId = 1;
        public List<PopulationItemDistribution> Populations { get; set; } = new List<PopulationItemDistribution>();
        public int NItemsToRecommend { get; set; }
        public RecommendableItem DefaultItem { get; set; }

        public PopulationDistributionCollection()
        {
        }

        public PopulationItemDistribution GetPopulation(int populationId)
        {
            foreach (var item in Populations)
            {
                if (item.Population_Id == populationId)
                {
                    return item;
                }
            }
            return null;
        }

        public void Add(PopulationItemDistribution population)
        {
            Populations.Add(population);
        }

        public int CalculatePopulationId(InvokeCategoricalOptimiserModel payload)
        {
            return ConstantPopulationId;
        }
    }
}