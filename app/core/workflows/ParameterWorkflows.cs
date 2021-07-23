using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Core.Workflows
{
    public class ParameterWorkflows : IWorkflow
    {
        private readonly ILogger<ParameterWorkflows> logger;
        private readonly IParameterStore parameterStore;
        private readonly IStorageContext storageContext;

        public ParameterWorkflows(ILogger<ParameterWorkflows> logger, IParameterStore parameterStore, IStorageContext storageContext)
        {
            this.logger = logger;
            this.parameterStore = parameterStore;
            this.storageContext = storageContext;
        }

        public async Task<Parameter> CreateParameter(CreateParameterModel model)
        {
            logger.LogInformation($"Creating parameter {model.Common.CommonId}");
            if (Enum.TryParse<ParameterTypes>(model.ParameterType, out var parameterType))
            {
                var p = await parameterStore.Create(new Parameter(model.Common, parameterType, model.Description));
                p.SetDefaultValue(model.DefaultValue);
                await storageContext.SaveChanges();
                return p;
            }
            else
            {
                throw new BadRequestException($"Cannot parse parameter type {model.ParameterType}");
            }
        }
    }
}