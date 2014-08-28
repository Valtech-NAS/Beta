namespace SFA.Apprenticeships.Service.Vacancy.Types
{
    using System;
    using System.Runtime.Serialization;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;

    [DataContract(Namespace = "http://candidates.gov.uk")]
    public class SearchResponse
    {
        [DataMember(Order = 1, Name = "Request")]
        public SearchRequest Request { get; set; }

        [DataMember(Order = 2, Name = "SearchResults")]
        public SearchResults<VacancySummaryResponse> SearchResults { get; set; }
    }
}
