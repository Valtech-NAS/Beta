using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace SFA.Apprenticeships.Services.Common.Configuration
{
    public class ConfigurationSettingsService : IConfigurationSettingsService
    {
        private readonly XmlNodeList _nodes;

        /// <summary>
        /// TODO::Medium::service needs caching decorator
        /// </summary>
        public ConfigurationSettingsService(string xml, ConfigurationSettingsType settings) 
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentNullException("xml");
            }

            var doc = new XmlDocument();
            switch (settings)
            {
                case ConfigurationSettingsType.File:
                    doc.Load(xml);
                    break;

                default:
                    doc.LoadXml(xml);
                    break;
            }

            _nodes = doc.SelectNodes("configuration/appSettings/add");
        }

        public string TryGetSetting(string key)
        {
            return _nodes.Cast<XmlNode>()
                .Where(node => node.Attributes["key"].Value.Equals(key, StringComparison.InvariantCultureIgnoreCase))
                .Select(node => node.Attributes["value"].Value)
                .SingleOrDefault();
        }

        public string GetSetting(string key)
        {
            var setting = TryGetSetting(key);
            if (setting == null)
            {
                throw new KeyNotFoundException(
                    string.Format("'{0}' was not found, or multiple values for the same key, in settings configuration file.", key));
            }

            return setting;
        }

        public T GetSetting<T>(string key)
        {
            var setting = GetSetting(key);
            return (T)Convert.ChangeType(setting, typeof(T));
        }
    }
}
