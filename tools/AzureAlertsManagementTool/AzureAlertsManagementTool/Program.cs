namespace AzureAlertsManagementTool
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Management.Monitoring.Alerts;
    using Microsoft.WindowsAzure.Management.Monitoring.Alerts.Models;
    using Microsoft.WindowsAzure.Management.Monitoring.Metrics;
    using Microsoft.WindowsAzure.Management.Monitoring.Utilities;

    internal class Program
    {
        private static string _alertEmailAddress;
        private static string _subscriptionId;
        private static string _thumbprint;
        private static string _cloudServiceName;
        private static string _deploymentName;

        private static void Main(string[] args)
        {
            _subscriptionId = args[0];
            _thumbprint = args[1];
            _alertEmailAddress = args[2];
            _cloudServiceName = args[3];
            _deploymentName = args[4];

            SubscriptionCloudCredentials credentials = new CertificateCloudCredentials(_subscriptionId,
                GetStoreCertificate(_thumbprint));

            var metricsClient = new MetricsClient(credentials);

            var resourceId = ResourceIdBuilder.BuildCloudServiceResourceId(_cloudServiceName, _deploymentName);

            Console.WriteLine("Resource Id: {0}", resourceId);

            GetMetricDefinitions(metricsClient, resourceId);

            var alertsClient = new AlertsClient(credentials);

            DisplayAzureAlertRules(alertsClient);

            var response = CreateAzureAlert(resourceId, alertsClient);

            Console.WriteLine("Create alert rule response: " + response.Result.StatusCode);
            Console.ReadLine();
        }

        private static void GetMetricDefinitions(MetricsClient metricsClient, string resourceId)
        {
            //Get the metric definitions.
            var retrieveMetricsTask =
                metricsClient.MetricDefinitions.List(resourceId, null, "");
            var metricListResponse = retrieveMetricsTask;

            var metricDefinitions = metricListResponse.MetricDefinitionCollection.Value;

            // Display the metric definitions.
            var count = 0;
            foreach (var metricDefinition in metricDefinitions)
            {
                Console.WriteLine("MetricDefinitio: " + count++);
                Console.WriteLine("Display Name: " + metricDefinition.DisplayName);
                Console.WriteLine("Metric Name: " + metricDefinition.Name);
                Console.WriteLine("Metric ResourceId Suffix: " + metricDefinition.ResourceIdSuffix);
                Console.WriteLine("Metric Namespace: " + metricDefinition.Namespace);
                Console.WriteLine("Is Altertable: " + metricDefinition.IsAlertable);
                Console.WriteLine("Min. Altertable Time Window: " + metricDefinition.MinimumAlertableTimeWindow);
                Console.WriteLine();
            }
        }

        private static void DisplayAzureAlertRules(AlertsClient alertsClient)
        {
            // Get the alert rules.
            var rulesResponse = alertsClient.Rules.List();
            var rules = rulesResponse.Value;

            // Display the alert rules.
            foreach (var rule in rules)
            {
                var ruleCondition = rule.Condition as ThresholdRuleCondition;
                var metricDs = ruleCondition.DataSource as RuleMetricDataSource;

                Console.WriteLine("Name: " + rule.Name);
                Console.WriteLine("Description: " + rule.Description);
                Console.WriteLine("Is enabled: " + rule.IsEnabled);
                Console.WriteLine("Last updated: " + rule.LastUpdatedTime);
                Console.WriteLine("Operator: " + ruleCondition.Operator
                                  + " Threshold: " + ruleCondition.Threshold + " Metric name: " + metricDs.MetricName);
                Console.WriteLine("Metric Namespace: " + metricDs.MetricNamespace);
                Console.WriteLine("ResourceId: " + metricDs.ResourceId);
                Console.WriteLine("");
            }
        }

        private static Task<OperationResponse> CreateAzureAlert(string resourceId, AlertsClient alertsClient)
        {
            var rule = new Rule
            {
                Name = "CPU Alert",
                Id = Guid.NewGuid().ToString(),
                Description = "If CPU usage is greater than twenty percent then alert",
                IsEnabled = true,
                Condition = new ThresholdRuleCondition
                {
                    Operator = ConditionOperator.GreaterThan,
                    Threshold = 10,
                    WindowSize = TimeSpan.FromMinutes(15), //new TimeSpan(0,0,5,0),
                    DataSource = new RuleMetricDataSource
                    {
                        MetricName = "Percentage CPU",
                        ResourceId = resourceId,
                        MetricNamespace = string.Empty //"WindowsAzure.Availability"
                    }
                }
            };

            RuleAction action = new RuleEmailAction
            {
                SendToServiceOwners = true
            };

            ((RuleEmailAction) action).CustomEmails.Add(_alertEmailAddress);
            rule.Actions.Add(action);
            rule.LastUpdatedTime = DateTime.UtcNow;

            var response = alertsClient.Rules.CreateOrUpdateAsync(new RuleCreateOrUpdateParameters
            {
                Rule = rule
            }, CancellationToken.None);

            return response;
        }

        private static X509Certificate2 GetStoreCertificate(string thumbprint)
        {
            var locations = new List<StoreLocation>
            {
                StoreLocation.CurrentUser,
                StoreLocation.LocalMachine
            };

            foreach (var location in locations)
            {
                var store = new X509Store("My", location);
                try
                {
                    store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                    var certificates = store.Certificates.Find(
                        X509FindType.FindByThumbprint, thumbprint, false);
                    if (certificates.Count == 1)
                    {
                        return certificates[0];
                    }
                }
                finally
                {
                    store.Close();
                }
            }
            throw new ArgumentException(string.Format(
                "A Certificate with Thumbprint '{0}' could not be located.",
                thumbprint));
        }
    }
}