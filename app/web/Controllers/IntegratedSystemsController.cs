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
    public class IntegratedSystemsController : EntityControllerBase<IntegratedSystem>
    {
        private readonly IntegratedSystemWorkflows workflows;

        public IntegratedSystemsController(IntegratedSystemWorkflows workflows, IIntegratedSystemStore store) : base(store)
        {
            this.workflows = workflows;
        }

        /// <summary>Creates a new Integrated System.</summary>
        [HttpPost]
        public async Task<IntegratedSystem> CreateIntegratedSystem(CreateIntegratedSystemDto dto)
        {
            return await workflows.CreateIntegratedSystem(dto.Name, dto.SystemType);
        }
    }
}