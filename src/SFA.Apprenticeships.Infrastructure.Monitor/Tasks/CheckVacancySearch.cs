namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies;
    using SFA.Apprenticeships.Application.Interfaces.Search;
    using SFA.Apprenticeships.Application.Interfaces.Vacancies;
    using SFA.Apprenticeships.Application.Vacancy;

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