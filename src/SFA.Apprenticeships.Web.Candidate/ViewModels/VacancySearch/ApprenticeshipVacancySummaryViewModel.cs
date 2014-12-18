namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using Domain.Entities.Vacancies.Apprenticeships;

    public class ApprenticeshipVacancySummaryViewModel : VacancySummaryViewModel
    {
        public ApprenticeshipLocationType VacancyLocationType { get; set; }
    }
}