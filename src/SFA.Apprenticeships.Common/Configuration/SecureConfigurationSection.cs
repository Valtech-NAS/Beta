namespace SFA.Apprenticeships.Common.Configuration
{
    using System.Configuration;

    public abstract class SecureConfigurationSection<T> : ConfigurationSection where T : ConfigurationSection, new()
    {
        private static T _instance;

        protected SecureConfigurationSection(string configSectionName)
        {
            // Needs the file (path and name) for the private configuration settings file to be set in web.config.
            var configFile = System.Configuration.ConfigurationManager.AppSettings[ConfigurationManager.ConfigurationFileAppSetting];
            var configMap = new ExeConfigurationFileMap { ExeConfigFilename = configFile };
            var config = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            _instance = (T)config.GetSection(configSectionName);
        }

        public static T Instance
        {
            get { return _instance ?? (_instance = new T()); }
        }
    }
}
