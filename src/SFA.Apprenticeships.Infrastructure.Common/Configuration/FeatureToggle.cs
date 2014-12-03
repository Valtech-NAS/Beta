namespace SFA.Apprenticeships.Infrastructure.Common.Configuration
{
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Microsoft.WindowsAzure;
    using SFA.Apprenticeships.Domain.Interfaces.Configuration;

    public class FeatureToggle : IFeatureToggle
    {
        private readonly Dictionary<string, string> _featureToggleKeys = new Dictionary<string, string>
        {
            {"Traineeships", "TraineeshipsEnabled"}
        };

        public bool IsActive(string featureName)
        {
            Condition.Ensures(featureName).IsNotNullOrWhiteSpace();

            if (!_featureToggleKeys.ContainsKey(featureName))
            {
                throw new KeyNotFoundException(string.Format("{0} was not found in feature toggle dictionary.",
                    featureName));
            }

            bool isActive;

            var settingValue = CloudConfigurationManager.GetSetting(_featureToggleKeys[featureName]);
            bool.TryParse(settingValue, out isActive);
            return isActive;
        }
    }
}