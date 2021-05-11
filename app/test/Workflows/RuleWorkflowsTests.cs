
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Infrastructure;
using SignalBox.Infrastructure.Services;
using Xunit;

namespace SignalBox.Test.Workflows
{
    public class RuleWorkflowsTests
    {
        private IDateTimeProvider dt = new SystemDateTimeProvider();

        [Fact]

        public async Task CanCreateRule()
        {
            // arrange
            var mock = new Mock<IStorageContext>();

            var ruleStore = new InMemoryRuleStore();
            
            var sut = new RuleWorkflows(mock.Object, ruleStore);

            //act
            var results = await sut.CreateRule("name", 1, "eventKey", "eventLogicalValue");

            //assert
            Assert.NotNull(results);
            var stored_rule = await ruleStore.Read(results.Id);
            Assert.NotNull(stored_rule);
            Assert.Equal(stored_rule.Name, results.Name);
            Assert.Equal(stored_rule.SegmentId, results.SegmentId);
            Assert.Equal(stored_rule.EventKey, results.EventKey);
            Assert.Equal(stored_rule.EventLogicalValue, results.EventLogicalValue);

            //verify SaveChanges() runs exactly once
            mock.Verify((m => m.SaveChanges()), Times.Once());
        }
    }
}
