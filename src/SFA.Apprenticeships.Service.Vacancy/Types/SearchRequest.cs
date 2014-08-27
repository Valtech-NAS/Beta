namespace SFA.Apprenticeships.Service.Vacancy.Types
{
    using System;
    using System.Collections.Generic;

    public class SearchRequest
    {
        public string TestRunId { get; set; }

        public string JobTitleTerms { get; set; }

        public string KeywordTerms { get; set; }

        public KeyValuePair<SearchParameters, string>[] Parameters { get; set; }
    }
}
