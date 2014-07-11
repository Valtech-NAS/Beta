namespace SFA.Apprenticeships.Infrastructure.Communication.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;
    using Common.Configuration;

    public class SendGridConfiguration : SecureConfigurationSection<SendGridConfiguration>
    {
        private const string UserNameConstant = "UserName";
        private const string PasswordConstant = "Password";

        public SendGridConfiguration()
            : base("SendGridConfiguration")
        {
        }

        [ConfigurationProperty(UserNameConstant, IsRequired = true)]
        public string UserName
        {
            get { return (string)this[UserNameConstant]; }
            set { this[UserNameConstant] = value; }
        }

        [ConfigurationProperty(PasswordConstant, IsRequired = true)]
        public string Password
        {
            get { return (string)this[PasswordConstant]; }
            set { this[PasswordConstant] = value; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(TemplateConfigurationCollection), AddItemName = "Template")]
        public TemplateConfigurationCollection Templates
        {
            get { return (TemplateConfigurationCollection)this[""]; }
        }

        public class TemplateConfigurationCollection : ConfigurationElementCollection,
            IEnumerable<SendGridTemplateConfiguration>
        {
            private readonly List<SendGridTemplateConfiguration> _elements;
            private readonly Dictionary<string, SendGridTemplateConfiguration> _elementDictionary;

            public TemplateConfigurationCollection()
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

            public new SendGridTemplateConfiguration this[string name]
            {
                get { return _elementDictionary.ContainsKey(name) ? _elementDictionary[name] : null; }
            }

            public new IEnumerator<SendGridTemplateConfiguration> GetEnumerator()
            {
                return _elements.GetEnumerator();
            }
        }
    }
}
