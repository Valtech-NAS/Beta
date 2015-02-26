namespace SFA.Apprenticeships.Infrastructure.VacancySearch.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;
    using Common.Configuration;

    public class SearchConfiguration : SecureConfigurationSection<SearchConfiguration>
    {
        private const string UseJobTitleTermsConst = "UseJobTitleTerms";
        private const string SearchJobTitleFieldConst = "SearchJobTitleField";
        private const string SearchDescriptionFieldConst = "SearchDescriptionField";
        private const string SearchEmployerNameFieldConst = "SearchEmployerNameField";
        public SearchConfiguration() : base("SearchConfiguration")
        {    
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(SearchTermFactorCollection), AddItemName = "SearchTermFactors")]
        public SearchTermFactorCollection SearchTermParameters
        {
            get { return (SearchTermFactorCollection)this[""]; }
        }

        [ConfigurationProperty(SearchJobTitleFieldConst, IsRequired = true, DefaultValue = true)]
        public bool SearchJobTitleField
        {
            get { return (bool)this[SearchJobTitleFieldConst]; }
            set { this[SearchJobTitleFieldConst] = value; }
        }

        [ConfigurationProperty(SearchDescriptionFieldConst, IsRequired = true, DefaultValue = true)]
        public bool SearchDescriptionField
        {
            get { return (bool)this[SearchDescriptionFieldConst]; }
            set { this[SearchDescriptionFieldConst] = value; }
        }

        [ConfigurationProperty(SearchEmployerNameFieldConst, IsRequired = true, DefaultValue = false)]
        public bool SearchEmployerNameField
        {
            get { return (bool)this[SearchEmployerNameFieldConst]; }
            set { this[SearchEmployerNameFieldConst] = value; }
        }
    }

    public class SearchTermFactorCollection : ConfigurationElementCollection, IEnumerable<ISearchTermFactorsConfiguration>
    {
        private readonly List<ISearchTermFactorsConfiguration> _elements;
        private readonly Dictionary<string, ISearchTermFactorsConfiguration> _elementDictionary;

        private ISearchTermFactorsConfiguration _jobTitleFactors;
        private ISearchTermFactorsConfiguration _descriptionFactors;
        private ISearchTermFactorsConfiguration _employerFactors;

        public SearchTermFactorCollection()
        {
            _elements = new List<ISearchTermFactorsConfiguration>();
            _elementDictionary = new Dictionary<string, ISearchTermFactorsConfiguration>();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            var element = new SearchTermFactorsConfiguration();
            _elements.Add(element);

            return element;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            string key = ((ISearchTermFactorsConfiguration)element).FieldName;
            if (!_elementDictionary.ContainsKey(key))
            {
                _elementDictionary.Add(key, ((ISearchTermFactorsConfiguration)element));
            }
            return _elementDictionary[key];
        }

        public new ISearchTermFactorsConfiguration this[string name]
        {
            get
            {
                return _elementDictionary.ContainsKey(name) ? _elementDictionary[name] : null;
            }
        }

        public new IEnumerator<ISearchTermFactorsConfiguration> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        public ISearchTermFactorsConfiguration JobTitleFactors
        {
            get {
                return _jobTitleFactors ??
                       (_jobTitleFactors = this["JobTitle"] ?? new SearchTermFactorsConfiguration());
            }
        }

        public ISearchTermFactorsConfiguration DescriptionFactors
        {
            get {
                return _descriptionFactors ??
                       (_descriptionFactors = this["Description"] ?? new SearchTermFactorsConfiguration());                
            }
        }

        public ISearchTermFactorsConfiguration EmployerFactors
        {
            get {
                return _employerFactors ??
                       (_employerFactors = this["Employer"] ?? new SearchTermFactorsConfiguration());                

            }
        }
    }
}
