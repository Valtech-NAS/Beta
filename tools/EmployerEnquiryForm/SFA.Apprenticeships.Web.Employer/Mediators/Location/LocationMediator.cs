namespace SFA.Apprenticeships.Web.Employer.Mediators.Location
{
    using EmployerEnquiry;
    using Interfaces;
    using Providers.Interfaces;
    using ViewModels;

    public class LocationMediator : MediatorBase, ILocationMediator
    {
        private ILocationProvider _locationProvider;

        public LocationMediator(  ILocationProvider locationProvider)
        {
            _locationProvider = locationProvider;
        }

        public MediatorResponse<LocationsViewModel> FindAddress(string postcode)
        {
            var result = _locationProvider.FindAddress(postcode);
            return GetMediatorResponse(EmployerEnquiryMediatorCodes.FindAddress.Success, result);
        }
    }
}