namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System.Collections.Generic;
    using SFA.Apprenticeships.Domain.Entities.Vacancy;

    public class VacancySearchResponseViewModel
    {
        public int Total { get; set; }

        public IEnumerable<VacancySummary> Vacancies { get; set; }
    }
}