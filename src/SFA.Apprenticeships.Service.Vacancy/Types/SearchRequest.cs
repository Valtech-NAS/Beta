namespace SFA.Apprenticeships.Service.Vacancy.Types
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract(Namespace = "http://candidates.gov.uk")]
    public class SearchRequest
    {
        [DataMember(Order = 1)]
        public string TestRunId { get; set; }

        [DataMember(Order = 2)]
        public string JobTitleTerms { get; set; }

        [DataMember(Order = 3)]
        public string KeywordTerms { get; set; }

        [DataMember(Order = 4)]
        public Dictionary<SearchParameters, string> Parameters { get; set; }
    }
}
