using System;
using System.Collections.Generic;
using System.Linq;
using SignalBox.Core;
using System.Text.Json.Serialization;

namespace SignalBox.Functions
{
    public class PopulationItemDistribution
    {
        private const double DefaultWeight = 0.5;
        private static Random _rnd = new Random();

        [JsonPropertyName("population_id")]
        public int Population_Id { get; set; }
        [JsonPropertyName("default_item")]
        public RecommendableItem Default_Item { get; set; }
        public List<RecommendableItem> Items { get; set; } = new List<RecommendableItem>();
        public Dictionary<string, double> Probabilities { get; set; }  = new Dictionary<string, double>();

        public PopulationItemDistribution()
        {
        }

        public PopulationItemDistribution(RecommendableItem defaultItem, IEnumerable<RecommendableItem> items, int populationId = 1 )
        {
            Population_Id = populationId;
            Default_Item = defaultItem;
            Items.AddRange(items);
            if (items == null || items.Count() == 0 )
            {
                Items.Add(Default_Item);
            }

            foreach (var item in Items)
            {
                double prob = 1.0; // item == DefaultItem
                
                if (item.CommonId != Default_Item?.CommonId)
                {
                    prob = 1.0 / (double)items.Count();
                }

                Probabilities[item.CommonId] = prob;
            }
        }

        public void NormalizeProbabilities()
        {
            Probabilities = NormalizeDictionary(Probabilities);
        }

        private Dictionary<string, double> NormalizeDictionary(Dictionary<string, double> dict)
        {
            Dictionary<string, double> newDict = new Dictionary<string, double>(dict);
            var total = 0.0;

            foreach (var item in dict)
            {
                total += item.Value;
            }

            foreach (var item in dict)
            {
                newDict[item.Key] = Math.Round(item.Value / total, 4);
            }

            return newDict;
        }

        
        public double GetItemProbability(string itemId)
        {
            double probability = 0.0;
            Probabilities.TryGetValue(itemId, out probability);
            return probability;
        }

    
        public List<ScoredRecommendableItem> ChooseItems(ICollection<RecommendableItem> items, int nitems)
        {
            if (items.Count <= 0)
            {
                throw new Exception("items must not be empty");
            }

            List<string> itemIds = new List<string>();
            foreach(var item in items)
            {
                itemIds.Add(item.CommonId);
            }

            Dictionary<string, double> newProbabilities = new Dictionary<string, double>();
            foreach(var item in items)
            {
                double probability = 0.0;
                if (Probabilities.TryGetValue(item.CommonId, out probability))
                {
                    newProbabilities[item.CommonId] = probability;
                }
                else
                {
                    double prob = 1.0; // item == DefaultItem                
                    if (item.CommonId != Default_Item?.CommonId)
                    {
                        prob = 1.0 / (double)items.Count();
                    }
                    newProbabilities[item.CommonId] = prob;
                }
            }

            Probabilities = NormalizeDictionary(newProbabilities);

            // sort collections
            SortedDictionary<string, double> choices = new SortedDictionary<string, double>(Probabilities);
            return SelectItems(items, choices, nitems);
        }

        public List<ScoredRecommendableItem> SelectItems(ICollection<RecommendableItem> items, SortedDictionary<string, double> probabilities, int nSelectedItems)
        {
            List<ScoredRecommendableItem> selectedItems = new List<ScoredRecommendableItem>();
            var cumulativeWeight = new List<double>();
            double last = 0;            
            foreach (var cur in probabilities)
            {
                last += cur.Value;
                cumulativeWeight.Add(last);
            }

            double minimum = 0.0;
            double choice = _rnd.NextDouble() * (last - minimum) + minimum;;
            int i = 0;
            foreach (var cur in probabilities)
            {
                if (choice < cumulativeWeight[i])
                {
                    var item = GetItem(items, cur.Key);
                    var newItem = new RecommendableItem(item.CommonId, item.Name, item.ListPrice, item.DirectCost, item.Properties);
                    selectedItems.Add(new ScoredRecommendableItem(newItem, cur.Value));
                }
                i++;
                if (selectedItems.Count == nSelectedItems)
                {
                    break;
                }
            }
            return selectedItems;
        }

        private RecommendableItem GetItem(ICollection<RecommendableItem> items, string key)
        {
            foreach(var item in items)
            {
                if (item.CommonId == key)
                {
                    return item;
                }
            }
            return null;
        }
    }
 
}