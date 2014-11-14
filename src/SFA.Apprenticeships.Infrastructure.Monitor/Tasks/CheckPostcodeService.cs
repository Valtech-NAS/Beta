namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using SFA.Apprenticeships.Application.Interfaces.Locations;

    public class CheckPostcodeService : IMonitorTask
    {
        private readonly IPostcodeLookupProvider _postcodeLookupProvider;

        public CheckPostcodeService(IPostcodeLookupProvider postcodeLookupProvider)
        {
            _postcodeLookupProvider = postcodeLookupProvider;
        }

        public string TaskName
        {
            get { return "Check postcode service"; }
        }

        public void Run()
        {
            _postcodeLookupProvider.GetLocation("EC1A 4JQ");
        }
    }
}
