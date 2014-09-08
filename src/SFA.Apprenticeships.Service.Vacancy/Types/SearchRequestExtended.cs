namespace SFA.Apprenticeships.Service.Vacancy.Types
{
    using System;
    using System.Collections.Generic;

    public class SearchRequestExtended : SearchRequest
    {
        public SearchRequestExtended(SearchRequest searchRequest)
        {
            TestRunId = searchRequest.TestRunId;
            JobTitleTerms = searchRequest.JobTitleTerms;
            KeywordTerms = searchRequest.KeywordTerms;

            UseJobTitleTerms = searchRequest.Parameters.ContainsKey(SearchParameters.UseJobTitleTerms) &&
                               bool.Parse(searchRequest.Parameters[SearchParameters.UseJobTitleTerms]);

            SearchJobTitleField = searchRequest.Parameters.ContainsKey(SearchParameters.SearchJobTitleField) &&
                                  bool.Parse(searchRequest.Parameters[SearchParameters.SearchJobTitleField]);

            SearchDescriptionField = searchRequest.Parameters.ContainsKey(SearchParameters.SearchDescriptionField) &&
                                     bool.Parse(searchRequest.Parameters[SearchParameters.SearchDescriptionField]);

            SearchEmployerNameField = searchRequest.Parameters.ContainsKey(SearchParameters.SearchEmployerNameField) &&
                                      bool.Parse(searchRequest.Parameters[SearchParameters.SearchEmployerNameField]);

            JobTitleFactors = new KeywordFactors(searchRequest.Parameters, SearchParameters.JobTitleBoost,
                SearchParameters.JobTitleFuzziness, SearchParameters.JobTitleFuzzyPrefix,
                SearchParameters.JobTitleMatchAllKeywords, SearchParameters.JobTitlePhraseProximity,
                SearchParameters.JobTitleMinimumMatch);

            DescriptionFactors = new KeywordFactors(searchRequest.Parameters, SearchParameters.DescriptionBoost,
                SearchParameters.DescriptionFuzziness, SearchParameters.DescriptionFuzzyPrefix,
                SearchParameters.DescriptionMatchAllKeywords, SearchParameters.DescriptionPhraseProximity,
                SearchParameters.DescriptionMinimumMatch);

            EmployerFactors = new KeywordFactors(searchRequest.Parameters, SearchParameters.EmployerNameBoost,
                SearchParameters.EmployerNameFuzziness, SearchParameters.EmployerNameFuzzyPrefix,
                SearchParameters.EmployerNameMatchAllKeywords, SearchParameters.EmployerNamePhraseProximity,
                SearchParameters.EmployerMinimumMatch);
        }

        public bool UseJobTitleTerms { get; set; }
        public bool SearchJobTitleField { get; set; }
        public bool SearchDescriptionField { get; set; }
        public bool SearchEmployerNameField { get; set; }

        public KeywordFactors JobTitleFactors { get; set; }
        public KeywordFactors DescriptionFactors { get; set; }
        public KeywordFactors EmployerFactors { get; set; }
    }

    public class KeywordFactors
    {
        public KeywordFactors(Dictionary<SearchParameters, string> parameters, 
            SearchParameters boost,
            SearchParameters fuzzy, 
            SearchParameters fuzzyPrefix, 
            SearchParameters matchAllKeywords,
            SearchParameters phraseProximity,
            SearchParameters minimumMatch)
        {

            Boost = parameters.ContainsKey(boost) ? new double?(double.Parse(parameters[boost])) : null;
            Fuzziness = parameters.ContainsKey(fuzzy) ? new int?(int.Parse(parameters[fuzzy])) : null;
            FuzzinessPrefix = parameters.ContainsKey(fuzzyPrefix) ? new int?(int.Parse(parameters[fuzzyPrefix])) : null;
            MatchAllKeywords = parameters.ContainsKey(matchAllKeywords) && bool.Parse(parameters[matchAllKeywords]);
            PhraseProximity = parameters.ContainsKey(phraseProximity) ? new int?(int.Parse(parameters[phraseProximity])) : null;
            MinimumMatch = parameters.ContainsKey(minimumMatch) ? parameters[minimumMatch] : null;
        }

        public double? Boost { get; set; }
        public int? Fuzziness { get; set; }
        public int? FuzzinessPrefix { get; set; }
        public bool MatchAllKeywords { get; set; }
        public int? PhraseProximity { get; set; }
        public string MinimumMatch { get; set; }
    }
}
