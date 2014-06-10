namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System.Collections.Generic;
    using SFA.Apprenticeships.Application.Interfaces.Vacancy;

    public class VacancySearchResponseViewModel
    {
        public int Total { get; set; }

        public IEnumerable<VacancySummaryResponse> Vacancies { get; set; }

        public VacancySearchViewModel VacancySearch { get; set; }
    }
}