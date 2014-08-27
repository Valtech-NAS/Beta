namespace SFA.Apprenticeships.Service.Vacancy.Types
{
    using System;
    using System.Collections.Generic;

    public class SearchRequestExtended : SearchRequest
    {
        public SearchRequestExtended(SearchRequest searchRequest)
        {
            this.TestRunId = searchRequest.TestRunId;
            this.JobTitleTerms = searchRequest.JobTitleTerms;
            this.KeywordTerms = searchRequest.KeywordTerms;

            this.UseJobTitleTerms = searchRequest.Parameters.ContainsKey(SearchParameters.UseJobTitleTerms) &&
                                    bool.Parse(searchRequest.Parameters[SearchParameters.UseJobTitleTerms]);

            this.SearchJobTitleField = searchRequest.Parameters.ContainsKey(SearchParameters.SearchJobTitleField) &&
                                       bool.Parse(searchRequest.Parameters[SearchParameters.SearchJobTitleField]);

            this.SearchDescriptionField = searchRequest.Parameters.ContainsKey(SearchParameters.SearchDescriptionField) &&
                                    bool.Parse(searchRequest.Parameters[SearchParameters.SearchDescriptionField]);

            this.SearchEmployerNameField = searchRequest.Parameters.ContainsKey(SearchParameters.SearchEmployerNameField) &&
                                    bool.Parse(searchRequest.Parameters[SearchParameters.SearchEmployerNameField]);

            JobTitleFactors = new KeywordFactors(searchRequest.Parameters, SearchParameters.JobTitleBoost,
                                    SearchParameters.JobTitleFuzziness, SearchParameters.JobTitleFuzzyPrefix,
                                    SearchParameters.JobTitleMatchAllKeywords, SearchParameters.JobTitlePhraseProximity,
                                    SearchParameters.JobTitlePhraseOrdering);

            DescriptionFactors = new KeywordFactors(searchRequest.Parameters, SearchParameters.DescriptionBoost,
                                    SearchParameters.DescriptionFuzziness, SearchParameters.DescriptionFuzzyPrefix,
                                    SearchParameters.DescriptionMatchAllKeywords, SearchParameters.DescriptionPhraseProximity,
                                    SearchParameters.DescriptionPhraseOrdering);

            EmployerFactors = new KeywordFactors(searchRequest.Parameters, SearchParameters.EmployerNameBoost,
                                    SearchParameters.EmployerNameFuzziness, SearchParameters.EmployerNameFuzzyPrefix,
                                    SearchParameters.EmployerNameMatchAllKeywords, SearchParameters.EmployerNamePhraseProximity,
                                    SearchParameters.EmployerNamePhraseOrdering);
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
            SearchParameters phaseOrdering)
        {

            Boost = parameters.ContainsKey(boost) ? double.Parse(parameters[boost]) : 1;
            Fuzziness = parameters.ContainsKey(fuzzy) ? int.Parse(parameters[fuzzy]) : 0;
            FuzzinessPrefix = parameters.ContainsKey(fuzzyPrefix) ? int.Parse(parameters[fuzzyPrefix]) : 0;
            MatchAllKeywords = parameters.ContainsKey(matchAllKeywords) ? bool.Parse(parameters[matchAllKeywords]) : true;
            PhraseProximity = parameters.ContainsKey(phraseProximity) ? int.Parse(parameters[phraseProximity]) : 1;
            PhraseOrdering = parameters.ContainsKey(phaseOrdering) ? int.Parse(parameters[phaseOrdering]) : 0;
        }

        public double Boost { get; set; }
        public int Fuzziness { get; set; }
        public int FuzzinessPrefix { get; set; }
        public bool MatchAllKeywords { get; set; }
        public int PhraseProximity { get; set; }
        public int PhraseOrdering { get; set; }
    }

}
