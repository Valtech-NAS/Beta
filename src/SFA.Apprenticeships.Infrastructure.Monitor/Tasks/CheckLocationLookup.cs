namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Application.Interfaces.Locations;
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
            catch (Exception exception)
            {
                Logger.ErrorException("Error while accessing Location lookup", exception);
            }

            Logger.Debug(string.Format("Finished running task {0}", TaskName));
        }
    }
}