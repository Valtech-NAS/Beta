namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
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
            _vacancySearchProvider.FindVacancies(string.Empty, new Location {GeoPoint = new GeoPoint()},
                1, 10, 10, VacancySortType.Distance, VacancyLocationType.National);
        }
    }
}