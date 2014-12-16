namespace SFA.Apprenticeships.Infrastructure.Common.Configuration
{
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Microsoft.WindowsAzure;
    using SFA.Apprenticeships.Domain.Interfaces.Configuration;

    public class FeatureToggle : IFeatureToggle
    {
        private readonly Dictionary<Feature, string> _featureToggleKeys = new Dictionary<Feature, string>
        {
            {Feature.Traineeships, "TraineeshipsEnabled"}
        };

        public bool IsActive(Feature feature)
        {
            if (!_featureToggleKeys.ContainsKey(feature))
            {
                throw new KeyNotFoundException(string.Format("{0} was not found in feature toggle dictionary.",
                    feature));
            }

            bool isActive;

            var settingValue = CloudConfigurationManager.GetSetting(_featureToggleKeys[feature]);
            bool.TryParse(settingValue, out isActive);
            return isActive;
        }
    }
}