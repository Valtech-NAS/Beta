namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Application.Vacancy;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies;

    public class CheckVacancySearch : IMonitorTask
    {
        private readonly IVacancySearchProvider _vacancySearchProvider;

        public CheckVacancySearch(IVacancySearchProvider vacancySearchProvider)
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