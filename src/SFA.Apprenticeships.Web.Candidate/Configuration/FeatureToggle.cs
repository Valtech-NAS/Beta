namespace SFA.Apprenticeships.Web.Candidate.Configuration
{
    using System.Collections.Generic;
    using Domain.Interfaces.Configuration;

    public class FeatureToggle : IFeatureToggle
    {
        private readonly IConfigurationManager _configurationManager;

        public FeatureToggle(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        private readonly Dictionary<Feature, string> _featureToggleKeys = new Dictionary<Feature, string>
        {
            {Feature.SavedSearches, "SavedSearchesEnabled"},
            {Feature.Sms, "SmsEnabled"}
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