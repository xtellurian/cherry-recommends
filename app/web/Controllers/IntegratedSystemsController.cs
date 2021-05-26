using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class IntegratedSystemsController : ControllerBase
    {
        private readonly IntegratedSystemWorkflows workflows;
        private readonly IIntegratedSystemStore integratedSystemStore;

        public IntegratedSystemsController(IntegratedSystemWorkflows workflows, IIntegratedSystemStore integratedSystemStore)
        {
            this.workflows = workflows;
            this.integratedSystemStore = integratedSystemStore;
        }

        [HttpPost]
        public async Task<IntegratedSystem> CreateIntegratedSystem(CreateIntegratedSystemDto dto)
        {
            return await workflows.CreateIntegratedSystem(dto.Name, dto.SystemType);
        }

        [HttpGet]
        public async Task<IEnumerable<IntegratedSystem>> ListIntegratedSystems()
        {
            return await integratedSystemStore.List();
        }
    }
}