namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using Application.Address;

    public class CheckAddressSearch : IMonitorTask
    {
        private readonly IAddressSearchProvider _addressSearchProvider;

        public CheckAddressSearch(IAddressSearchProvider addressSearchProvider)
        {
            _addressSearchProvider = addressSearchProvider;
        }

        public string TaskName
        {
            get { return "Check address search"; }
        }

        public void Run()
        {
            _addressSearchProvider.FindAddress("EC1A 4JQ");
        }
    }
}
