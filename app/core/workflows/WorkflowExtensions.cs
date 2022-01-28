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
            services.AddScoped<MetricGeneratorWorkflows>();
            services.AddScoped<MetricWorkflows>();
            services.AddScoped<RecommendableItemWorkflows>();
            services.AddScoped<SegmentWorkflows>();
            services.AddScoped<CustomerWorkflows>();
            services.AddScoped<CustomerEventsWorkflows>();
            services.AddScoped<DataSummaryWorkflows>();
            services.AddScoped<TouchpointWorkflows>();
            services.AddScoped<WebhookWorkflows>();
            services.AddScoped<IntegratedSystemWorkflows>();
            services.AddScoped<HubspotWorkflows>();
            services.AddScoped<HubspotEtlWorkflows>();
            services.AddScoped<HubspotPushWorkflows>();
            services.AddScoped<ParameterWorkflows>();
            services.AddScoped<ParameterSetRecommenderWorkflows>();
            services.AddScoped<ItemsRecommenderWorkflows>();
            // images and blob reports
            services.AddScoped<ReportWorkflows>();
            services.AddScoped<RecommenderReportImageWorkflows>();

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