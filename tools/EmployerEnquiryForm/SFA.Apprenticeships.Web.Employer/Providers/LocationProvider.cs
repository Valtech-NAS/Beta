namespace SFA.Apprenticeships.Web.Employer.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces;
    using Domain.Entities;
    using Domain.Extensions;
    using Interfaces;
    using Mappers.Interfaces;
    using ViewModels;

    public class LocationProvider : ILocationProvider
    {
        private ILocationSearchService _locationSearchService;
        private IDomainToViewModelMapper<Location, LocationViewModel> _locationDomainToViewModelMapper;

        public LocationProvider(ILocationSearchService locationSearchService, IDomainToViewModelMapper<Location, LocationViewModel> locationDomainToViewModelMapper)
        {
            _locationSearchService = locationSearchService;
            _locationDomainToViewModelMapper = locationDomainToViewModelMapper;
        }

        public LocationsViewModel FindAddress(string postcode)
        {
            try
            {
                var addressData = _locationSearchService.FindAddress(postcode);

                var addressDataViewModel = addressData.Select(m => _locationDomainToViewModelMapper.ConvertToViewModel(m));

                IEnumerable<LocationViewModel> locationViewModels = addressDataViewModel as IList<LocationViewModel> ?? addressDataViewModel.ToList();
                if (!locationViewModels.IsNullOrEmpty())
                {
                    return new LocationsViewModel(locationViewModels);
                }
                return new LocationsViewModel();
            }
            catch (System.Exception exception)
            {
                //todo: log error using preferred logging mechanism
                return new LocationsViewModel();
            }
        }
    }
}