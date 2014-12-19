namespace SFA.Apprenticeships.Infrastructure.Common.Configuration
{
    using System.Collections.Generic;
    using Microsoft.WindowsAzure;
    using SFA.Apprenticeships.Domain.Interfaces.Configuration;

    public class FeatureToggle : IFeatureToggle
    {
        private readonly IConfigurationManager _configurationManager;

        public FeatureToggle(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

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

            return _configurationManager.GetCloudAppSetting<bool>(_featureToggleKeys[feature]);
        }
    }
}