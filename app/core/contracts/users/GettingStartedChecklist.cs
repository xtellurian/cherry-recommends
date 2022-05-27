using System.Collections.Generic;
using System.Linq;

namespace SignalBox.Core.Internal
{
    public class GettingStartedChecklist
    {
        private static Dictionary<int, CheckistItem> InitialChecklist => new Dictionary<int, CheckistItem>
        {
            {
                11,
                new CheckistItem
                {
                    label= "Connect Cherry to your Stack",
                    description= "Choose an integration from the Cherry App Library.",
                    actionTo= "/settings/integrations/create#datasources",
                    actionLabel= "Connect",
                    docsLink="/docs/integrations/library",
                    complete = false
                }
            },
            {
                22,
                new CheckistItem
                {
                    label= "Create a Promotion",
                    description= "For example, 10% off next purchase.",
                    actionTo= "/promotions/promotions/create",
                    actionLabel= "Create Promotion",
                    docsLink="/docs/guides/create_promotions",
                    complete = false,
                }
            },
            {
                32,
                new CheckistItem
                {
                    label= "Create a Campaign",
                    description= "A Promotion Campaign chooses the best promotion for every customer. "
                     + "For example, a campaign can choose the best promotion for returning customers to your landing pages.",
                    actionTo= "/campaigns/promotions-campaigns/create",
                    actionLabel= "Setup",
                    docsLink="/docs/guides/create_promotion_campaign",
                    complete = false,
                }
            },
            {
                52,
                new CheckistItem
                {
                    label= "Deliver Promotions via Channels",
                    description= "Recommendations are delivered to Customers via Channels. "
                        + "Channels may be your website, an email campaign, or backend systems.",
                    actionTo= "/integrations/channels/create",
                    actionLabel= "Deliver",
                    docsLink="/docs/integrations/library",
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