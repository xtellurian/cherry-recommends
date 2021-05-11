using System.Threading.Tasks;

namespace SignalBox.Core.Workflows
{
    public class RuleWorkflows
    {
        private readonly IStorageContext storageContext;
        private readonly IRuleStore ruleStore;

        public RuleWorkflows(IStorageContext storageContext, IRuleStore ruleStore)
        {
            this.storageContext = storageContext;
            this.ruleStore = ruleStore;
        }
        public async Task<Rule> CreateRule(string name, long segmentId, string eventKey, string eventLogicalValue)
        {
            var rule = await ruleStore.Create(new Rule(name, segmentId, eventKey, eventLogicalValue));
            await storageContext.SaveChanges();
            return rule;
        }

        
    }
}