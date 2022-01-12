using Microsoft.Extensions.DependencyInjection;

namespace SignalBox.Core.Workflows
{
    public static class WorkflowExtensions
    {
        public static IServiceCollection RegisterWorkflows(this IServiceCollection services)
        {
            services.AddScoped<ApiKeyWorkflows>();
            services.AddScoped<IntegratedSystemWorkflows>();
            services.AddScoped<ModelRegistrationWorkflows>();
            services.AddScoped<FeatureGeneratorWorkflows>();
            services.AddScoped<FeatureWorkflows>();
            services.AddScoped<RecommendableItemWorkflows>();
            services.AddScoped<SegmentWorkflows>();
            services.AddScoped<CustomerWorkflows>();
            services.AddScoped<CustomerEventsWorkflows>();
            services.AddScoped<DataSummaryWorkflows>();
            services.AddScoped<ReportWorkflows>();
            services.AddScoped<TouchpointWorkflows>();
            services.AddScoped<WebhookWorkflows>();
            services.AddScoped<IntegratedSystemWorkflows>();
            services.AddScoped<HubspotWorkflows>();
            services.AddScoped<HubspotEtlWorkflows>();
            services.AddScoped<HubspotPushWorkflows>();
            services.AddScoped<ParameterWorkflows>();
            services.AddScoped<ParameterSetRecommenderWorkflows>();
            services.AddScoped<ItemsRecommenderWorkflows>();

            // model invokation
            services.AddScoped<GenericModelWorkflows>();
            services.AddScoped<ClassifierModelWorkflows>();
            services.AddScoped<ItemsRecommenderInvokationWorkflows>();
            services.AddScoped<ParameterSetRecommenderInvokationWorkflows>();

            services.AddScoped<RecommenderTriggersWorkflows>();
            return services;
        }
    }
}