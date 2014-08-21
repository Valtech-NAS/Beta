namespace SFA.Apprenticeships.Service.Vacancy.Types
{
    /// <summary>
    /// List of parameter names that can be passed in and their meaning.
    /// 
    /// UseJobTitleTerms            True or False
    ///                             Determines which input terms to use when searching the job title data field.
    ///                             If True will use job title terms
    ///                             If False will use the keywords terms
    /// 
    /// SearchJobTitleField         If True will search this field
    /// 
    /// SearchDescriptionField      If True will search this field
    /// 
    /// SearchEmployerNameField     If True will search this field
    /// 
    /// -----------------------------------------------------------------------------------------------------
    /// 
    /// Boost               e.g. 1.0
    ///                     Inflated priority of this field compared to other fields
    /// 
    /// Fuzziness           e.g. 1
    ///                     Number of character changes allowed
    ///                     e.g. Support matches Sport with 2 changes
    /// 
    /// FuzzyPrefix         e.g. 1
    ///                     Number of leading characters that will NOT be affected by fuzzines
    ///                     e.g. value of 1 means first letter cannot be changed when applying fuzziness
    ///                     e.g. value of 0 means first letter may be changed when applying fuzziness
    /// 
    /// MatchAllKeywords    True or False
    ///                     If True then will match if ALL search terms are present
    ///                     If False then will match if ANY search terms are present
    /// 
    /// PhraseProximity     How close each of the search terms need to be in order to match
    ///                     see http://www.elasticsearch.org/guide/en/elasticsearch/guide/current/phrase-matching.html
    /// 
    /// PhraseOrdering      How important is the order of the search terms
    ///                     see http://www.elasticsearch.org/guide/en/elasticsearch/guide/current/slop.html
    ///                     see http://www.elasticsearch.org/guide/en/elasticsearch/guide/current/_closer_is_better.html
    /// 
    /// </summary>
    public enum SearchParameters
    {
        UseJobTitleTerms,

        SearchJobTitleField,
        SearchDescriptionField,
        SearchEmployerNameField,

        JobTitleBoost,
        JobTitleFuzziness,
        JobTitleFuzzyPrefix,
        JobTitleMatchAllKeywords,
        JobTitlePhraseProximity,
        JobTitlePhraseOrdering,

        DescriptionBoost,
        DescriptionFuzziness,
        DescriptionFuzzyPrefix,
        DescriptionMatchAllKeywords,
        DescriptionPhraseProximity,
        DescriptionPhraseOrdering,

        EmployerNameBoost,
        EmployerNameFuzziness,
        EmployerNameFuzzyPrefix,
        EmployerNameMatchAllKeywords,
        EmployerNamePhraseProximity,
        EmployerNamePhraseOrdering,

        // Later... synonyms, stemming
    }
}
