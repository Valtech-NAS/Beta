namespace SFA.Apprenticeships.Service.Vacancy.Types
{
    using System;
    using Application.Interfaces.Vacancies;

    public class SearchResponse
    {
        public SearchRequest Request { get; set; }

        public VacancySummaryResponse[] Results { get; set; }
    }
}
