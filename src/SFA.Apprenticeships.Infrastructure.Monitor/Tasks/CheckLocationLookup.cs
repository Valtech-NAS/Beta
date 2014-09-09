namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Application.Interfaces.Locations;
    using Application.Interfaces.Vacancies;
    using Application.Vacancy;
    using Domain.Entities.Locations;
    using NLog;

    public class CheckLocationLookup : IMonitorTask
    {
        private const string TaskName = "Check location lookup";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ILocationLookupProvider _locationLookupProvider;

        public CheckLocationLookup(ILocationLookupProvider locationLookupProvider)
        {
            _locationLookupProvider = locationLookupProvider;
        }

        public void Run()
        {
            Logger.Debug(string.Format("Start running task {0}", TaskName));

            try
            {
                _locationLookupProvider.FindLocation("London");
            }
            catch (Exception execption)
            {
                Logger.ErrorException("Error while accessing Location lookup", execption);
            }

            Logger.Debug(string.Format("Finished running task {0}", TaskName));
        }
    }
}