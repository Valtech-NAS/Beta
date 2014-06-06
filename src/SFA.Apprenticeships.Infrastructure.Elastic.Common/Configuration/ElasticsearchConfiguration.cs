namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using SFA.Apprenticeships.Infrastructure.Common.Configuration;

    public class ElasticsearchConfiguration : SecureConfigurationSection<ElasticsearchConfiguration>
    {
        private const string DefaultHostConst = "DefaultHost";
        public ElasticsearchConfiguration() : base("ElasticsearchConfiguration")
        {    
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ElasticsearchIndexCollection), AddItemName = "Index")]
        public ElasticsearchIndexCollection Indexes
        {
            get { return (ElasticsearchIndexCollection)this[""]; }
        }

        [ConfigurationProperty(DefaultHostConst, IsRequired = true)]
        public Uri DefaultHost
        {
            get { return (Uri)this[DefaultHostConst]; }
            set { this[DefaultHostConst] = value; }
        }
    }

    public class ElasticsearchIndexCollection : ConfigurationElementCollection, IEnumerable<IElasticsearchIndexConfiguration>
    {
        private readonly List<IElasticsearchIndexConfiguration> elements;
        private readonly Dictionary<string, IElasticsearchIndexConfiguration> elementDictionary;

        public ElasticsearchIndexCollection()
        {
            elements = new List<IElasticsearchIndexConfiguration>();
            elementDictionary = new Dictionary<string, IElasticsearchIndexConfiguration>();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            var element = new ElasticsearchIndexConfiguration();
            elements.Add(element);

            return element;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            string key = ((IElasticsearchIndexConfiguration)element).Name;
            if (!elementDictionary.ContainsKey(key))
            {
                elementDictionary.Add(key, ((ElasticsearchIndexConfiguration)element));
            }
            return elementDictionary[key];
        }

        public new IElasticsearchIndexConfiguration this[string name]
        {
            get
            {
                return elementDictionary.ContainsKey(name) ? elementDictionary[name] : null;
            }
        }

        public new IEnumerator<IElasticsearchIndexConfiguration> GetEnumerator()
        {
            return elements.GetEnumerator();
        }
    }
}
