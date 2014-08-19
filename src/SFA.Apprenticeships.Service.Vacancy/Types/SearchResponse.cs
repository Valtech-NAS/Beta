namespace SFA.Apprenticeships.Service.Vacancy.Types
{
    using System;

    public class SearchResponse
    {
        public SearchRequest Request { get; set; }

        public VacancySummary[] Results { get; set; }
    }
}
