using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class RulesController : ControllerBase
    {
        private readonly ILogger<RulesController> _logger;
        private readonly IRuleStore ruleStore;
        private readonly RuleWorkflows workflows;

        public RulesController(ILogger<RulesController> logger, IRuleStore ruleStore, RuleWorkflows workflows)
        {
            _logger = logger;
            this.ruleStore = ruleStore;
            this.workflows = workflows;
        }

        [HttpPost]
        public async Task<Rule> CreateRule([FromBody] CreateRuleDto dto)
        {
            return await workflows.CreateRule(dto.Name, dto.SegmentId, dto.EventKey, dto.EventLogicalValue);
        }

        [HttpGet]
        public async Task<IEnumerable<Rule>> GetRules(long? segmentId)
        {
            var rules = await ruleStore.List();
            if (segmentId == null)
            {
                return rules;
            }
            else
            {
                return rules.Where(_ => _.SegmentId == segmentId);
            }
        }

        [HttpGet("{id}")]
        public async Task<Rule> GetRule(long id)
        {
            return await ruleStore.Read(id);
        }
    }

}
