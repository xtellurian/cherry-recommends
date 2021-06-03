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
    public class ModelRegistrationsController : EntityControllerBase<ModelRegistration>
    {
        private readonly IStorageContext storageContext;
        private readonly ModelRegistrationWorkflows workflows;

        public ModelRegistrationsController(IStorageContext storageContext, ModelRegistrationWorkflows workflows, IModelRegistrationStore store) : base(store)
        {
            this.storageContext = storageContext;
            this.workflows = workflows;
        }

        /// <summary>Register a new model.</summary>
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
    }
}