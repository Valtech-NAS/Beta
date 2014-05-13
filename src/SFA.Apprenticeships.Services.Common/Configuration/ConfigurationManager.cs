using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;

namespace SFA.Apprenticeships.Services.Common.Configuration
{
    public class ConfigurationManager : IConfigurationManager
    {
        public static string ConfigurationFileAppSetting = "ConfigurationPath";

        private readonly System.Configuration.Configuration _config;

        /// <summary>
        /// TODO::Medium::service needs caching decorator
        /// </summary>
        public ConfigurationManager(string configFile) 
        {
            if (string.IsNullOrEmpty(configFile))
            {
                throw new ArgumentNullException("configFile");
            }

            var configMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = configFile
            };

            _config = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
        }

        public string TryGetAppSetting(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            var result = _config.AppSettings.Settings[key];
            if (result != null)
            {
                return result.Value;
            }

            // Create exception for logging.
            //var ex =
            //    new ApplicationException(
            //        string.Format(
            //            CultureInfo.InvariantCulture,
            //            "The value for '{0}' could not be found in the configuration file", key));
            // needs logging here

            return null;
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
            return (T)Convert.ChangeType(setting, typeof(T));
        }

        public T GetAppSetting<T>(T defaultValue, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            string result = this.TryGetAppSetting(key);
            if (result != null)
            {
                return (T)Convert.ChangeType(result, typeof(T), CultureInfo.InvariantCulture);
            }

            return defaultValue;
        }

        public ConfigurationSection GetSection(string sectionName)
        {
            return _config.GetSection(sectionName);
        }

        public ConnectionStringSettings GetConnectionString(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            var result = _config.ConnectionStrings.ConnectionStrings[key];
            if (result == null)
            {
                throw new ApplicationException(
                    string.Format(
                        "The connection string '{0}' could not be found in the configuration file",
                        key));
            }

            return result;
        }
    }
}
