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
                21,
                new CheckistItem
                {
                    label= "Create a Promotion",
                    description= "For example, 10% off next purchase.",
                    actionTo= "/promotions/create",
                    actionLabel= "Create",
                    docsLink="/docs/recommenders/choose-what-to-recommend/create_promotions",
                    complete = false,
                }
            },
            {
                31,
                new CheckistItem
                {
                    label= "Setup a Recommender",
                    description= "A Recommender provides the best promotion for every customer or business.",
                    actionTo= "/recommenders/promotions-recommenders/create",
                    actionLabel= "Setup",
                    docsLink="/docs/recommenders/creating/create-promotion-recommender",
                    complete = false,
                }
            },
            {
                51,
                new CheckistItem
                {
                    label= "Consume Recommendations",
                    description= "Ensure the recommendations are delivered to your customers and the results tracked.",
                    actionTo= "/recommenders/promotions-recommenders/advanced/{recommenderId}?tab=advanced",
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