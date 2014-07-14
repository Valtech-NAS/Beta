namespace SFA.Apprenticeships.Infrastructure.Communication.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;

    public class SendGridTemplateConfigurationCollection : ConfigurationElementCollection, IEnumerable<SendGridTemplateConfiguration>
    {
        private readonly List<SendGridTemplateConfiguration> _elements;
        private readonly Dictionary<string, SendGridTemplateConfiguration> _elementDictionary;

        public SendGridTemplateConfigurationCollection()
        {
            _elements = new List<SendGridTemplateConfiguration>();
            _elementDictionary = new Dictionary<string, SendGridTemplateConfiguration>();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            var element = new SendGridTemplateConfiguration();

            _elements.Add(element);

            return element;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var key = ((SendGridTemplateConfiguration)element).Name;

            if (!_elementDictionary.ContainsKey(key))
            {
                _elementDictionary.Add(key, ((SendGridTemplateConfiguration)element));
            }

            return _elementDictionary[key];
        }

        public new IEnumerator<SendGridTemplateConfiguration> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }
    }
}
