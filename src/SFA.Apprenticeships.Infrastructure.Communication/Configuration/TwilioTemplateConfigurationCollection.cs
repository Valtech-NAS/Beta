namespace SFA.Apprenticeships.Infrastructure.Communication.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;

    public class TwilioTemplateConfigurationCollection : ConfigurationElementCollection, IEnumerable<TwilioTemplateConfiguration>
    {
        private readonly List<TwilioTemplateConfiguration> _elements;
        private readonly Dictionary<string, TwilioTemplateConfiguration> _elementDictionary;

        public TwilioTemplateConfigurationCollection()
        {
            _elements = new List<TwilioTemplateConfiguration>();
            _elementDictionary = new Dictionary<string, TwilioTemplateConfiguration>();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            var element = new TwilioTemplateConfiguration();

            _elements.Add(element);

            return element;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var key = ((TwilioTemplateConfiguration)element).Name;

            if (!_elementDictionary.ContainsKey(key))
            {
                _elementDictionary.Add(key, ((TwilioTemplateConfiguration)element));
            }

            return _elementDictionary[key];
        }

        public new IEnumerator<TwilioTemplateConfiguration> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }
    }
}
