using System;
using System.Configuration;
using System.Globalization;

namespace SFA.Apprenticeships.Services.Common.Configuration
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IConfigurationSettingsService _settingsService;

        /// <summary>
        /// The default configuration manager with no access to private settings.
        /// </summary>
        public ConfigurationManager() { }

        /// <summary>
        /// Provides a mechanism for getting app settings from a private configuration store.
        /// </summary>
        public ConfigurationManager(IConfigurationSettingsService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            _settingsService = service;
        }

        /// <summary>
        /// Gets a string representation of the value located by the supplied key.
        /// If the value is not found, null is returned
        /// </summary>
        /// <param name="key">The key to find the required value.</param>
        /// <returns>The value located by the supplied key</returns>
        public string TryGetAppSetting(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            string result = System.Configuration.ConfigurationManager.AppSettings[key];
            if (result == null)
            {
                if (_settingsService != null)
                {
                    result = _settingsService.TryGetSetting(key);
                }

                if (result == null)
                {
                    // Create exception for logging.
                    var ex =
                        new ApplicationException(
                            string.Format(
                                CultureInfo.InvariantCulture,
                                "The value for '{0}' could not be found in the configuration file", key));
                    // needs logging here
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a string representation of the value located by the supplied key.
        /// If the value is not found, an exception is thrown.
        /// </summary>
        /// <param name="key">The key to find the required value.</param>
        /// <returns>The value located by the supplied key</returns>
        public string GetAppSetting(string key)
        {
            string result = this.TryGetAppSetting(key);
            if (result == null)
            {
                throw new ArgumentException("key", string.Format(CultureInfo.InvariantCulture, "No value exists in the current configuration file for {0}", key));
            }

            return result;
        }

        /// <summary>
        /// Gets the app setting.
        /// </summary>
        /// <typeparam name="T">The type of return</typeparam>
        /// <param name="key">The setting key.</param>
        /// <returns>
        /// The app setting strongly typed
        /// </returns>
        public T GetAppSetting<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            // If result is null throws argument exception in GetApppSetting.
            string result = this.GetAppSetting(key);
            return (T)Convert.ChangeType(result, typeof(T), CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets the value located by the supplied key.
        /// If the value is not found the default value is returned.
        /// </summary>
        /// <typeparam name="T">The type of the value to be returned</typeparam>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="key">The key to find the required value.</param>
        /// <returns>
        /// The value located by the supplied key
        /// </returns>
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

        #region IConfigurationHelper methods

        /// <summary>
        /// Gets the connection string identified by the supplied key.
        /// If the value is not found, null is returned
        /// </summary>
        /// <param name="key">The key (name) of the connection string to find.</param>
        /// <returns>
        /// The connectionstring located by the supplied key
        /// </returns>
        public ConnectionStringSettings GetConnectionString(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            var result = System.Configuration.ConfigurationManager.ConnectionStrings[key];
            if (result == null)
            {
                throw new ApplicationException(
                    string.Format(
                        "ConfigurationUtilities::GetConnectionString The connection string '{0}' could not be found in the configuration file",
                        key));
            }

            return result;
        }

        /// <summary>
        /// Gets the requested configuration section from the configuration file.
        /// </summary>
        /// <param name="sectionName">The name of the configuration section to retrieve</param>
        /// <returns>The found configuration section.</returns>
        public ConfigurationSection GetSection(string sectionName)
        {
            var result = (ConfigurationSection)System.Configuration.ConfigurationManager.GetSection(sectionName);
            return result;
        }
        #endregion
    }
}