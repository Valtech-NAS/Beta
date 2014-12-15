namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Application.Vacancy;

    public class CheckVacancySearch : IMonitorTask
    {
        private readonly IVacancySearchProvider<VacancySummaryResponse> _vacancySearchProvider;

        public CheckVacancySearch(IVacancySearchProvider<VacancySummaryResponse> vacancySearchProvider)
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
                VacancyLocationType = VacancyLocationType.National
            };

            _vacancySearchProvider.FindVacancies(parameters);
        }
    }
}