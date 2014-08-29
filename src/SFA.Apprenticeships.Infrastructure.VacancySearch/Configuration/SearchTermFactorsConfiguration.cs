namespace SFA.Apprenticeships.Infrastructure.VacancySearch.Configuration
{
    using System.Configuration;

    public class SearchTermFactorsConfiguration : ConfigurationElement, ISearchTermFactorsConfiguration
    {
        private const string FieldNameConst = "FieldName";
        private const string BoostConst = "Boost";
        private const string FuzzinessConst = "Fuzziness";
        private const string FuzzyPrefixConst = "FuzzyPrefix";
        private const string MatchAllKeywordsConst = "MatchAllKeywords";
        private const string PhraseProximityConst = "PhraseProximity";
        private const string MinimumMatchConst = "MinimumMatch";

        [ConfigurationProperty(FieldNameConst, IsRequired = true, IsKey = true)]
        public string FieldName
        {
            get { return (string)this[FieldNameConst]; }
            set { this[FieldNameConst] = value; }
        }

        [ConfigurationProperty(BoostConst, IsRequired = false, IsKey = false, DefaultValue = null)]
        public double? Boost
        {
            get { return (double?)this[BoostConst]; }
            set { this[BoostConst] = value; }
        }

        [ConfigurationProperty(FuzzinessConst, IsRequired = false, IsKey = false, DefaultValue = null)]
        public int? Fuzziness
        {
            get { return (int?)this[FuzzinessConst]; }
            set { this[FuzzinessConst] = value; } 
        }

        [ConfigurationProperty(FuzzyPrefixConst, IsRequired = false, IsKey = false, DefaultValue = null)]
        public int? FuzzyPrefix
        {
            get { return (int?)this[FuzzyPrefixConst]; }
            set { this[FuzzyPrefixConst] = value; } 
        }

        [ConfigurationProperty(MatchAllKeywordsConst, IsRequired = false, IsKey = false, DefaultValue = false)]
        public bool MatchAllKeywords
        {
            get { return (bool)this[MatchAllKeywordsConst]; }
            set { this[MatchAllKeywordsConst] = value; } 
        }

        [ConfigurationProperty(PhraseProximityConst, IsRequired = false, IsKey = false, DefaultValue = null)]
        public int? PhraseProximity
        {
            get { return (int?)this[PhraseProximityConst]; }
            set { this[PhraseProximityConst] = value; } 
        }

        [ConfigurationProperty(MinimumMatchConst, IsRequired = false, IsKey = false, DefaultValue = null)]
        public int? MinimumMatch
        {
            get { return (int?)this[MinimumMatchConst]; }
            set { this[MinimumMatchConst] = value; } 
        }
    }
}