namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Application.Interfaces.Vacancies;
    using Application.Vacancy;
    using Domain.Entities.Locations;
    using NLog;

    public class CheckVacancySearch : IMonitorTask
    {
        private const string TaskName = "Check vacancy search";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IVacancySearchProvider _vacancySearchProvider;

        public CheckVacancySearch(IVacancySearchProvider vacancySearchProvider)
        {
            _vacancySearchProvider = vacancySearchProvider;
        }

        public void Run()
        {
            Logger.Debug(string.Format("Start running task {0}", TaskName));

            try
            {
                _vacancySearchProvider.FindVacancies(string.Empty, new Location{GeoPoint = new GeoPoint()}, 
                    1, 10, 10, VacancySortType.Distance);
            }
            catch (Exception exception)
            {
                Logger.ErrorException("Error while accessing Vacancy search", exception);
            }

            Logger.Debug(string.Format("Finished running task {0}", TaskName));
        }
    }
}