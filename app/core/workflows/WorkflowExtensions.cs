using Microsoft.Extensions.DependencyInjection;

namespace SignalBox.Core.Workflows
{
    public static class WorkflowExtensions
    {
        public static IServiceCollection RegisterWorkflows(this IServiceCollection services)
        {
            services.AddScoped<ApiKeyWorkflows>();
            services.AddScoped<ModelRegistrationWorkflows>();
            services.AddScoped<MetricGeneratorWorkflows>();
            services.AddScoped<AggregateCustomerMetricWorkflow>();
            services.AddScoped<JoinTwoMetricsWorkflow>();
            services.AddScoped<MetricWorkflows>();
            services.AddScoped<RecommendableItemWorkflows>();
            services.AddScoped<CustomerSegmentWorkflows>();
            services.AddScoped<CustomerWorkflows>();
            services.AddScoped<CustomerEventsWorkflows>();
            services.AddScoped<DataSummaryWorkflows>();
            services.AddScoped<WebhookWorkflows>();
            services.AddScoped<HubspotWorkflows>();
            services.AddScoped<HubspotEtlWorkflows>();
            services.AddScoped<HubspotPushWorkflows>();
            services.AddScoped<ParameterWorkflows>();
            services.AddScoped<ParameterSetRecommenderWorkflows>();
            services.AddScoped<PromotionsRecommenderWorkflows>();
            services.AddScoped<ItemsRecommenderPerformanceWorkflows>();
            services.AddScoped<BusinessWorkflows>();
            services.AddScoped<BusinessMetricGeneratorWorkflow>();
            services.AddScoped<FilterSelectAggregateWorkflow>();
            services.AddScoped<SegmentEnrolmentWorkflow>();
            services.AddScoped<ChannelWorkflow>();
            // images and blob reports
            services.AddScoped<ReportWorkflows>();

            // model invokation
            services.AddScoped<GenericModelWorkflows>();
            services.AddScoped<ClassifierModelWorkflows>();
            services.AddScoped<ItemsRecommenderInvokationWorkflows>();
            services.AddScoped<ParameterSetRecommenderInvokationWorkflows>();

            services.AddScoped<AggregateCustomerMetricWorkflow>();

            // also register any interfaces that are implemented by workflows
            services.AddScoped<ICustomerWorkflow, CustomerWorkflows>();
            services.AddScoped<ICustomerEventsWorkflow, CustomerEventsWorkflows>();
            services.AddScoped<IBusinessWorkflow, BusinessWorkflows>();
            services.AddScoped<ICustomerSegmentWorkflow, CustomerSegmentWorkflows>();
            services.AddScoped<IPromotionOptimiserCRUDWorkflow, PromotionOptimiserCRUDWorkflow>();
            services.AddScoped<IRecommenderReportImageWorkflow, RecommenderReportImageWorkflows>();
            services.AddScoped<IRecommenderMetricTriggersWorkflow, RecommenderTriggersWorkflows>();
            services.AddScoped<IChannelWorkflow, ChannelWorkflow>();
            services.AddScoped<IShopifyAdminWorkflow, ShopifyAdminWorkflows>();
            services.AddScoped<IShopifyWebhookWorkflow, ShopifyWebhookWorkflow>();
            services.AddScoped<ISegmentWebhookWorkflow, SegmentWebhookWorkflow>();
            services.AddScoped<IIntegratedSystemWorkflow, IntegratedSystemWorkflows>();
            services.AddScoped<IDiscountCodeWorkflow, DiscountCodeWorkflows>();
            services.AddScoped<IKlaviyoSystemWorkflow, KlaviyoSystemWorkflow>();
            services.AddScoped<IOfferWorkflow, OfferWorkflows>();
            return services;
        }
    }
}