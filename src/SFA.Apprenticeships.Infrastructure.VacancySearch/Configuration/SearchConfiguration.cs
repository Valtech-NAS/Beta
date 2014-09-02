namespace SFA.Apprenticeships.Infrastructure.VacancySearch.Configuration
{
    using System;
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

        [ConfigurationProperty(UseJobTitleTermsConst, IsRequired = true, DefaultValue = false)]
        public bool UseJobTitleTerms
        {
            get { return (bool)this[UseJobTitleTermsConst]; }
            set { this[UseJobTitleTermsConst] = value; }
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
        private readonly List<ISearchTermFactorsConfiguration> elements;
        private readonly Dictionary<string, ISearchTermFactorsConfiguration> elementDictionary;

        private ISearchTermFactorsConfiguration jobTitleFactors = null;
        private ISearchTermFactorsConfiguration descriptionFactors = null;
        private ISearchTermFactorsConfiguration employerFactors = null;

        public SearchTermFactorCollection()
        {
            elements = new List<ISearchTermFactorsConfiguration>();
            elementDictionary = new Dictionary<string, ISearchTermFactorsConfiguration>();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            var element = new SearchTermFactorsConfiguration();
            elements.Add(element);

            return element;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            string key = ((ISearchTermFactorsConfiguration)element).FieldName;
            if (!elementDictionary.ContainsKey(key))
            {
                elementDictionary.Add(key, ((ISearchTermFactorsConfiguration)element));
            }
            return elementDictionary[key];
        }

        public new ISearchTermFactorsConfiguration this[string name]
        {
            get
            {
                return elementDictionary.ContainsKey(name) ? elementDictionary[name] : null;
            }
        }

        public new IEnumerator<ISearchTermFactorsConfiguration> GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        public ISearchTermFactorsConfiguration JobTitleFactors
        {
            get {
                return jobTitleFactors ??
                       (jobTitleFactors = this["JobTitle"] ?? new SearchTermFactorsConfiguration());
            }
        }

        public ISearchTermFactorsConfiguration DescriptionFactors
        {
            get {
                return descriptionFactors ??
                       (descriptionFactors = this["Description"] ?? new SearchTermFactorsConfiguration());                
            }
        }

        public ISearchTermFactorsConfiguration EmployerFactors
        {
            get {
                return employerFactors ??
                       (employerFactors = this["Employer"] ?? new SearchTermFactorsConfiguration());                

            }
        }
    }
}
