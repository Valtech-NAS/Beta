namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Application.Interfaces.Locations;
    using NLog;

    public class CheckAddressSearch : IMonitorTask
    {
        private const string TaskName = "Check address search";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IAddressSearchProvider _addressSearchProvider;

        public CheckAddressSearch(IAddressSearchProvider addressSearchProvider)
        {
            _addressSearchProvider = addressSearchProvider;
        }

        public void Run()
        {
            Logger.Debug(string.Format("Start running task {0}", TaskName));

            try
            {
                _addressSearchProvider.FindAddress("EC1A 4JQ");
            }
            catch (Exception exception)
            {
                Logger.ErrorException("Error while accessing Address search", exception);
            }

            Logger.Debug(string.Format("Finished running task {0}", TaskName));
        }
    }
}