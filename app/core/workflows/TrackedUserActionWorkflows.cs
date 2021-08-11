using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Core.Workflows
{
    public class TrackedUserActionWorkflows : IWorkflow
    {
        private readonly ITrackedUserActionStore actionStore;
        private readonly IRewardSelectorStore rewardSelectorStore;
        private readonly ILogger<TrackedUserActionWorkflows> logger;
        private readonly IStorageContext storageContext;

        public TrackedUserActionWorkflows(ITrackedUserActionStore actionStore,
                                          IRewardSelectorStore rewardSelectorStore,
                                          ILogger<TrackedUserActionWorkflows> logger,
                                          IStorageContext storageContext)
        {
            this.actionStore = actionStore;
            this.rewardSelectorStore = rewardSelectorStore;
            this.logger = logger;
            this.storageContext = storageContext;
        }

        public async Task<IEnumerable<TrackedUserAction>> ProcessActionsFromEvents(IEnumerable<TrackedUserEvent> events, bool saveChanges = false)
        {
            var actions = new List<TrackedUserAction>();
            foreach (var a in events.ToActions())
            {
                actions.Add(await actionStore.Create(a));
            }

            var distinctActionNames = actions.Select(_ => _.ActionName).Distinct();
            var selectorDic = new Dictionary<string, IEnumerable<RewardSelector>>();
            foreach (var uniqueActionName in distinctActionNames)
            {
                var selectors = await rewardSelectorStore.GetSelectorsForActionName(uniqueActionName);
                if (selectors.Any())
                {
                    selectorDic.Add(uniqueActionName, selectors);
                }
            }

            // now process the selector rules
            foreach (var action in actions.Where(_ => selectorDic.ContainsKey(_.ActionName)))
            {
                var selectors = selectorDic[action.ActionName];
                foreach (var selector in selectors)
                {
                    switch (selector.SelectorType)
                    {
                        case SelectorTypes.Revenue:
                            ProcessRevenueSelector(selector, action);
                            break;
                        default:
                            logger.LogWarning($"Unhandled Selector: {selector.Id}");
                            break;

                    }
                }
            }

            if (saveChanges)
            {
                await storageContext.SaveChanges();
            }

            return actions;
        }

        private void ProcessRevenueSelector(RewardSelector selector, TrackedUserAction action)
        {
            bool shouldProcess = false;
            if (string.IsNullOrEmpty(action.ActionValue))
            {
                shouldProcess = false; // can't process null values
            }
            else if (selector.Category != null)
            {
                shouldProcess = string.Equals(action.ActionName, selector.ActionName)
                    && string.Equals(action.Category, selector.Category);
            }
            else
            {
                shouldProcess = string.Equals(action.ActionName, selector.ActionName); // ignore category, because it's null
            }

            if (shouldProcess)
            {
                // apply the rule
                if (Double.TryParse(action.ActionValue, out var revenue))
                {
                    action.AssociatedRevenue = revenue;
                }
                else
                {
                    logger.LogWarning($"Couldn't parse revenue event property: {action.ActionValue}.");
                }
            }
        }

        public async Task<Paginated<ActionCategoryAndName>> ReadTrackedUserCategoriesAndActionNames(int page, string commonUserId)
        {
            return await actionStore.ReadTrackedUserCategoriesAndActionNames(page, commonUserId);
        }

        public async Task<TrackedUserAction> ReadLatestAction(string commonUserId, string category, string actionName = null)
        {
            return await actionStore.ReadLatestAction(commonUserId, category, actionName);
        }
    }
}