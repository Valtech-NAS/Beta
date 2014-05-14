namespace SFA.Apprenticeships.Common.Configuration
{
    using System.Configuration;

    public abstract class SecureConfigurationSection<T> : ConfigurationSection where T : ConfigurationSection, new()
    {
        private static T _instance;
        private static string _configFile;
        private static string _configSectionName;

        protected SecureConfigurationSection(string configSectionName)
        {
            // Needs the file (path and name) for the private configuration settings file to be set in web.config.
            _configFile = System.Configuration.ConfigurationManager.AppSettings[ConfigurationManager.ConfigurationFileAppSetting];
            _configSectionName = configSectionName;
        }

        public static T Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                var configObj = new T();
                var configMap = new ExeConfigurationFileMap { ExeConfigFilename = _configFile };
                var config = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                _instance = (T) config.GetSection(_configSectionName);

                return _instance;
            }
        }
    }
}
