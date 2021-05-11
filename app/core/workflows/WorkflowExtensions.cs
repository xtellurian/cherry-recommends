using Microsoft.Extensions.DependencyInjection;

namespace SignalBox.Core.Workflows
{
    public static class WorkflowExtensions
    {
        public static IServiceCollection RegisterWorkflows(this IServiceCollection services)
        {
            services.AddScoped<ExperimentWorkflows>();
            services.AddScoped<OfferWorkflows>();
            services.AddScoped<RuleWorkflows>();
            services.AddScoped<SegmentWorkflows>();
            services.AddScoped<TrackedUserWorkflows>();
            services.AddScoped<PresentationsWorkflows>();
            services.AddScoped<ApiKeyWorkflows>();
            return services;
        }
    }
}