namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using Application.Location;

    public class CheckLocationLookup : IMonitorTask
    {
        private readonly ILocationLookupProvider _locationLookupProvider;

        public CheckLocationLookup(ILocationLookupProvider locationLookupProvider)
        {
            _locationLookupProvider = locationLookupProvider;
        }

        public string TaskName
        {
            get { return "Check location lookup"; }
        }

        public void Run()
        {
            _locationLookupProvider.FindLocation("London");
        }
    }
}
