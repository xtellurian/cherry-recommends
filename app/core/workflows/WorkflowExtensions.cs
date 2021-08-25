using Microsoft.Extensions.DependencyInjection;

namespace SignalBox.Core.Workflows
{
    public static class WorkflowExtensions
    {
        public static IServiceCollection RegisterWorkflows(this IServiceCollection services)
        {
            services.AddScoped<ApiKeyWorkflows>();
            services.AddScoped<ExperimentWorkflows>();
            services.AddScoped<IntegratedSystemWorkflows>();
            services.AddScoped<ModelRegistrationWorkflows>();
            services.AddScoped<OfferWorkflows>();
            services.AddScoped<FeatureGeneratorWorkflows>();
            services.AddScoped<FeatureWorkflows>();
            services.AddScoped<PresentationsWorkflows>();
            services.AddScoped<ProductWorkflows>();
            services.AddScoped<RuleWorkflows>();
            services.AddScoped<SegmentWorkflows>();
            services.AddScoped<TrackedUserWorkflows>();
            services.AddScoped<TrackedUserEventsWorkflows>();
            services.AddScoped<TrackedUserActionWorkflows>();
            services.AddScoped<DataSummaryWorkflows>();
            services.AddScoped<ReportWorkflows>();
            services.AddScoped<TouchpointWorkflows>();
            services.AddScoped<WebhookWorkflows>();
            services.AddScoped<IntegratedSystemWorkflows>();
            services.AddScoped<HubspotWorkflows>();
            services.AddScoped<ParameterWorkflows>();
            services.AddScoped<ParameterSetRecommenderWorkflows>();
            services.AddScoped<ProductRecommenderWorkflows>();

            // model invokation
            services.AddScoped<GenericModelWorkflows>();
            services.AddScoped<ClassifierModelWorkflows>();
            services.AddScoped<ProductRecommenderInvokationWorkflows>();
            services.AddScoped<ParameterSetRecommenderInvokationWorkflows>();
            return services;
        }
    }
}