using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;
using SignalBox.Web.Dto.ModelRegistration;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class ModelRegistrationsController : ControllerBase
    {
        private readonly IStorageContext storageContext;
        private readonly ModelRegistrationWorkflows workflows;
        private readonly IModelRegistrationStore modelRegistrationStore;

        public ModelRegistrationsController(IStorageContext storageContext, ModelRegistrationWorkflows workflows, IModelRegistrationStore modelRegistrationStore)
        {
            this.storageContext = storageContext;
            this.workflows = workflows;
            this.modelRegistrationStore = modelRegistrationStore;
        }

        [HttpPost]
        public async Task<ModelRegistration> RegisterNewModel(RegisterNewModelDto dto)
        {
            var model = await workflows.RegisterNewModel(dto.Name,
                                                    new Uri(dto.ScoringUrl),
                                                    new Uri(dto.SwaggerUrl),
                                                    dto.ModelType,
                                                    dto.HostingType,
                                                    dto.Key);
            await storageContext.SaveChanges();
            return model;
        }

        [HttpPost("{id}/evaluation")]
        public async Task<EvaluationResult> EvaluateModel(long id, [FromBody] Dictionary<string, object> features)
        {
            return await workflows.EvaluateModel(id, features);
        }

        [HttpGet]
        public async Task<IEnumerable<ModelRegistration>> ListModelRegistrations()
        {
            return await modelRegistrationStore.List();
        }
    }
}