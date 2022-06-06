
using System.Collections.Generic;
using Pulumi;
using Pulumi.AzureNative.Insights;
using Pulumi.AzureNative.Insights.Inputs;
using Pulumi.AzureNative.Resources;

namespace SignalBox.Azure
{
    class AppObservation : ComponentWithStorage
    {
        public AppObservation(ResourceGroup rg, Dictionary<string, string> tags)
        {
            var observationsConfig = new Config("observations");
            var threshold = observationsConfig.GetInt32("exceptionsThreshold") ?? 100;

            AppInsights = new Component("Insights",
                new ComponentArgs
                {
                    ApplicationType = "web",
                    Kind = "web",
                    ResourceGroupName = rg.Name,
                    Tags = tags,
                });

            this.engineeringAG = new ActionGroup("engineeringAG", new Pulumi.AzureNative.Insights.ActionGroupArgs
            {
                ActionGroupName = Pulumi.Deployment.Instance.StackName + "_AllEngineering",
                GroupShortName = "AllEng",
                ResourceGroupName = rg.Name,
                Location = "global",
                AzureAppPushReceivers =
                {
                    new AzureAppPushReceiverArgs
                    {
                        EmailAddress = "rian@cherry.ai",
                        Name = "Rian",
                    },
                    new AzureAppPushReceiverArgs
                    {
                        EmailAddress = "john@cherry.ai",
                        Name = "John Ephraim",
                    },
                    new AzureAppPushReceiverArgs
                    {
                        EmailAddress = "cherry.gonzales@cherry.ai",
                        Name = "Cherry G",
                    },
                    new AzureAppPushReceiverArgs
                    {
                        EmailAddress = "quim@cherry.ai",
                        Name = "Quim",
                    },
                },
            });

            var metricAlert = new MetricAlert("metricAlert", new MetricAlertArgs
            {
                Actions =
                {
                    new  MetricAlertActionArgs
                    {
                        ActionGroupId = engineeringAG.Id,
                    },
                },
                AutoMitigate = true,
                Criteria = new Dictionary<string, object>
                    {
                        { "odataType", "Microsoft.Azure.Monitor.SingleResourceMultipleMetricCriteria" },
                        { "allOf", new MetricCriteriaArgs[1] {  new MetricCriteriaArgs
                        {
                            CriterionType = "StaticThresholdCriterion",
                            Dimensions = {},
                            MetricName = "exceptions/count",
                            Name = $"Exceptions are high in {Pulumi.Deployment.Instance.StackName}",
                            Operator = "GreaterThan",
                            Threshold = threshold,
                            TimeAggregation = AggregationTypeEnum.Count,
                        }} },
                    },
                Description = $"Check exceptions aren't too high in {Pulumi.Deployment.Instance.StackName}",
                Enabled = true,
                EvaluationFrequency = "PT30M",
                Location = "global",
                ResourceGroupName = rg.Name,
                RuleName = "CheckExceptions",
                Scopes =
                {
                    AppInsights.Id,
                },
                Severity = 1,
                WindowSize = "PT1H",
            });

        }

        public Component AppInsights { get; }

        private readonly ActionGroup engineeringAG;
    }
}
