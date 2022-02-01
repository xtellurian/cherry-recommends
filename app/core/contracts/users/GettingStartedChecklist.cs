using System.Collections.Generic;
using System.Linq;

namespace SignalBox.Core.Internal
{
    public class GettingStartedChecklist
    {
        private static Dictionary<int, CheckistItem> InitialChecklist => new Dictionary<int, CheckistItem>
        {
            {
                10,
                new CheckistItem
                {
                    label= "Connect Cherry",
                    description= "Integrate Cherry to your application.",
                    actionTo= "/settings/integrations/create",
                    actionLabel= "Connect",
                    docsLink="/docs/integrations/sources/segment",
                    complete = false
                }
            },
            {
                20,
                new CheckistItem
                {
                    label= "Create an item",
                    description= "Add any promotions, offers, features, or content you want to show to your customers.",
                    actionTo= "/recommendable-items/create",
                    actionLabel= "Create",
                    docsLink="/docs/recommenders/choose-what-to-recommend/create_items",
                    complete = false,
                }
            },
            {
                30,
                new CheckistItem
                {
                    label= "Setup a Recommender",
                    description= "A Recommender provides the best item for each of your customers.",
                    actionTo= "/recommenders/items-recommenders/create",
                    actionLabel= "Setup",
                    docsLink="/docs/recommenders/Create/create-item-recommender",
                    complete = false,
                }
            },
            // {
            //     40,
            //     new CheckistItem
            //     {
            //         label= "Find your Recommender in the list",
            //         description= "Cherry can contain multiple recommenders. Try finding your recommender in the list.",
            //         actionTo= "/recommenders/items-recommenders",
            //         actionLabel= "Find your recommender",
            //         complete = false
            //     }
            // },
            {
                50,
                new CheckistItem
                {
                    label= "Consume Recommendations",
                    description= "Ensure the recommendations are delivered to your customers and the results tracked.",
                    actionTo= "/recommenders/items-recommenders/destinations/{recommenderId}?tab=js",
                    actionLabel= "Consume",
                    docsLink="/docs/recommenders/deploy-recommenders/consume/consume-recommendations",
                    complete = false
                }
            },
        };

        public void EnsureInitialised()
        {
            steps ??= InitialChecklist;

            foreach (var key in steps.Keys.ToList())
            {
                if (!InitialChecklist.ContainsKey(key))
                {
                    // remove any keys not in _initialChecklist
                    steps.Remove(key);
                }
            }

            var minIndex = steps.Keys.Min();
            var minIncompleteIndex = steps.Where(_ => _.Value.complete != true).Select(_ => _.Key).DefaultIfEmpty(int.MaxValue).Min();
            var maxIndex = steps.Keys.Max();

            foreach (var kvp in steps)
            {
                kvp.Value.current = kvp.Key == minIncompleteIndex;
                kvp.Value.next = kvp.Key > minIncompleteIndex;
            }

        }

        public Dictionary<int, CheckistItem> steps { get; set; } = InitialChecklist;
        public bool? allComplete => !steps?.Values.Any(_ => (_.complete == false) || (_.complete == null));
    }
}