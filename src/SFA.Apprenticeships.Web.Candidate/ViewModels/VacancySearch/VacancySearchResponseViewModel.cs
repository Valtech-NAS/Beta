namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using PagedList;
    using SFA.Apprenticeships.Application.Interfaces.Vacancy;

    public class VacancySearchResponseViewModel
    {
        public int Total { get; set; }

        public IPagedList<VacancySummaryResponse> Vacancies { get; set; }

        public VacancySearchViewModel VacancySearch { get; set; }
    }
}