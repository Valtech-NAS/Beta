namespace SFA.Apprenticeships.Common.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;

    public class ConfigurationManager : IConfigurationManager
    {
        internal const string ConfigurationFileAppSettingName = "ConfigurationPath";

        private Configuration Configuration { get; set; }

        public ConfigurationManager()
        {
            var configMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = ConfigurationFilePath
            };

            Configuration = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
        }

        public T GetCloudAppSetting<T>(string key)
        {
            throw new NotImplementedException();
        }

        public string TryGetAppSetting(string key)
        {
            var result = Configuration.AppSettings.Settings[key];

            return result != null ? result.Value : null;
        }

        public string ConfigurationFilePath
        {
            get
            {
                var configFile = System.Configuration.ConfigurationManager.AppSettings[ConfigurationFileAppSettingName];

                if (!File.Exists(configFile))
                {
                    configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile);
                }

                if (!File.Exists(configFile))
                {
                    throw new ConfigurationErrorsException(
                        string.Format("Configuration file: {0} does not exist", (object) configFile));
                }

                return configFile;
            }
        }

        public string GetAppSetting(string key)
        {
            var setting = TryGetAppSetting(key);
            if (setting == null)
            {
                throw new KeyNotFoundException(
                    string.Format("'{0}' was not found, or multiple values for the same key, in settings configuration file.", key));
            }

            return setting;
        }

        public T GetAppSetting<T>(string key)
        {
            var setting = GetAppSetting(key);
            return (T) Convert.ChangeType(setting, typeof (T));
        }
        
        public ConfigurationSection GetSection(string sectionName)
        {
            return Configuration.GetSection(sectionName);
        }
    }
}
