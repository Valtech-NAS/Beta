namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using Infrastructure.Common.Configuration;

    public class ElasticsearchConfiguration : SecureConfigurationSection<ElasticsearchConfiguration>
    {
        private const string DefaultHostConst = "DefaultHost";
        private const string NodeCountConst = "NodeCount";
        private const string TimeoutConst = "Timeout";
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

        [ConfigurationProperty(NodeCountConst, IsRequired = true)]
        public int NodeCount
        {
            get { return (int)this[NodeCountConst]; }
            set { this[NodeCountConst] = value; }
        }

        [ConfigurationProperty(TimeoutConst, IsRequired = true)]
        public int Timeout
        {
            get { return (int)this[TimeoutConst]; }
            set { this[TimeoutConst] = value; }
        }
    }

    public class ElasticsearchIndexCollection : ConfigurationElementCollection, IEnumerable<IElasticsearchIndexConfiguration>
    {
        private readonly List<IElasticsearchIndexConfiguration> _elements;
        private readonly Dictionary<string, IElasticsearchIndexConfiguration> _elementDictionary;

        public ElasticsearchIndexCollection()
        {
            _elements = new List<IElasticsearchIndexConfiguration>();
            _elementDictionary = new Dictionary<string, IElasticsearchIndexConfiguration>();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            var element = new ElasticsearchIndexConfiguration();
            _elements.Add(element);

            return element;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            string key = ((IElasticsearchIndexConfiguration)element).Name;
            if (!_elementDictionary.ContainsKey(key))
            {
                _elementDictionary.Add(key, ((ElasticsearchIndexConfiguration)element));
            }
            return _elementDictionary[key];
        }

        public new IElasticsearchIndexConfiguration this[string name]
        {
            get
            {
                return _elementDictionary.ContainsKey(name) ? _elementDictionary[name] : null;
            }
        }

        public new IEnumerator<IElasticsearchIndexConfiguration> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }
    }
}
