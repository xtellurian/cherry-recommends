using System.Collections.Generic;
using System.Linq;

namespace SignalBox.Core.Internal
{
    public class GettingStartedChecklist
    {
        private static Dictionary<int, CheckistItem> _initialChecklist => new Dictionary<int, CheckistItem>
        {
            {
                10,
                new CheckistItem
                {
                    label= "Connect your data",
                    description= "Add a Segment integration to automatically populate Cherry with customer data.",
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
                    label= "Add a recommendable item",
                    description= "Add a promotion or product as a recommendable item.",
                    actionTo= "/recommendable-items/create",
                    actionLabel= "Add an Item",
                    docsLink="/docs/recommenders/choose-what-to-recommend/create_items",
                    complete = false,
                }
            },
            {
                30,
                new CheckistItem
                {
                    label= "Setup a Recommender",
                    description= "Create an item recommender to start finding the best items for different customers.",
                    actionTo= "/recommenders/items-recommenders/create",
                    actionLabel= "Setup a Recommender",
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
                    label= "Consume the Recommendations",
                    description= "Your recommendations need to go somewhere. Use the JavaScript SDK to consume them in your web application.",
                    actionTo= "/recommenders/items-recommenders/destinations/{recommenderId}?tab=js",
                    actionLabel= "Consume Recommendations",
                    docsLink="/docs/recommenders/deploy-recommenders/consume/consume-recommendations",
                    complete = false
                }
            },
        };

        public void EnsureInitialised()
        {
            steps ??= _initialChecklist;

            foreach (var key in steps.Keys.ToList())
            {
                if (!_initialChecklist.ContainsKey(key))
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

        public Dictionary<int, CheckistItem> steps { get; set; } = _initialChecklist;
        public bool? allComplete => !steps?.Values.Any(_ => (_.complete == false) || (_.complete == null));
    }
}