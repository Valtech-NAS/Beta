namespace SFA.Apprenticeships.Infrastructure.RabbitMq.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;
    using Common.Configuration;

    public class RabbitMqHostsConfiguration : SecureConfigurationSection<RabbitMqHostsConfiguration>
    {
        private const string DefaultHostConst = "DefaultHost";
        public RabbitMqHostsConfiguration() : base("RabbitMQHosts")
        {    
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(RabbitMqCollection), AddItemName = "RabbitHost")]
        public RabbitMqCollection RabbitHosts
        {
            get { return (RabbitMqCollection)this[""]; }
        }

        [ConfigurationProperty(DefaultHostConst, IsRequired = true)]
        public string DefaultHost
        {
            get { return (string)this[DefaultHostConst]; }
            set { this[DefaultHostConst] = value; }
        }
    }

    public class RabbitMqCollection : ConfigurationElementCollection, IEnumerable<IRabbitMqHostConfiguration>
    {
        private readonly List<IRabbitMqHostConfiguration> elements;
        private readonly Dictionary<string, IRabbitMqHostConfiguration> elementDictionary;

        public RabbitMqCollection()
        {
            elements = new List<IRabbitMqHostConfiguration>();
            elementDictionary = new Dictionary<string, IRabbitMqHostConfiguration>();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            var element = new RabbitMqHostConfiguration();
            elements.Add(element);

            return element;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            string key = ((IRabbitMqHostConfiguration)element).Name;
            if (!elementDictionary.ContainsKey(key))
            {
                elementDictionary.Add(key, ((RabbitMqHostConfiguration)element));
            }
            return elementDictionary[key];
        }

        public new IRabbitMqHostConfiguration this[string name]
        {
            get
            {
                return elementDictionary.ContainsKey(name) ? elementDictionary[name] : null;
            }
        }

        public new IEnumerator<IRabbitMqHostConfiguration> GetEnumerator()
        {
            return elements.GetEnumerator();
        }
    }
}
