namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using Application.Interfaces.Locations;
    using NLog;

    public class CheckPostcodeService : IMonitorTask
    {
        private const string TaskName = "Check postcode service";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IPostcodeLookupProvider _postcodeLookupProvider;

        public CheckPostcodeService(IPostcodeLookupProvider postcodeLookupProvider)
        {
            _postcodeLookupProvider = postcodeLookupProvider;
        }

        public void Run()
        {
            Logger.Debug(string.Format("Start running task {0}", TaskName));

            try
            {
                _postcodeLookupProvider.GetLocation("EC1A 4JQ");
            }
            catch (Exception execption)
            {
                Logger.ErrorException("Error while accessing Postcode lookup", execption);
            }

            Logger.Debug(string.Format("Finished running task {0}", TaskName));
        }
    }
}