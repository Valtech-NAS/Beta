namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Application.Vacancy;
    using Domain.Entities.Vacancies.Apprenticeships;

    public class CheckVacancySearch : IMonitorTask
    {
        private readonly IVacancySearchProvider<ApprenticeshipSummaryResponse> _vacancySearchProvider;

        public CheckVacancySearch(IVacancySearchProvider<ApprenticeshipSummaryResponse> vacancySearchProvider)
        {
            _vacancySearchProvider = vacancySearchProvider;
        }

        public string TaskName
        {
            get { return "Check vacancy search"; }
        }

        public void Run()
        {
            var parameters = new SearchParameters
            {
                Keywords = string.Empty,
                Location = new Location {GeoPoint = new GeoPoint()},
                PageNumber = 1,
                PageSize = 10,
                SearchRadius = 10,
                SortType = VacancySortType.Distance,
                VacancyLocationType = ApprenticeshipLocationType.National
            };

            _vacancySearchProvider.FindVacancies(parameters);
        }
    }
}