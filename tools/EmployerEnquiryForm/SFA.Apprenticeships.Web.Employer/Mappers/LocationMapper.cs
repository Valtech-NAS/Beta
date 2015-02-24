namespace SFA.Apprenticeships.Web.Employer.Mappers
{
    using System.Collections.Generic;
    using Apprenticeships.Common.Extensions;
    using Domain.Entities;
    using Interfaces;
    using ViewModels;

    public class LocationMapper : IDomainToViewModelMapper<Location, LocationViewModel>, IViewModelToDomainMapper<LocationViewModel, Location>
    {
        private IDomainToViewModelMapper<Address, AddressViewModel> _addressDomainToViewModelMapper;
        private IViewModelToDomainMapper<AddressViewModel, Address> _addressViewModelToDomainMapper;

        public LocationMapper(IDomainToViewModelMapper<Address, AddressViewModel> addressDomainToViewModelMapper, IViewModelToDomainMapper<AddressViewModel, Address> addressViewModelToDomainMapper)
        {
            _addressDomainToViewModelMapper = addressDomainToViewModelMapper;
            _addressViewModelToDomainMapper = addressViewModelToDomainMapper;
        }

        public LocationViewModel ConvertToViewModel(Location domain)
        {
            domain.ThrowIfNull("Location", "domain object of type Location can't be null");

            return new LocationViewModel()
            {
                Address = _addressDomainToViewModelMapper.ConvertToViewModel(domain.Address),
                Latitude = domain.Latitude,
                Longitude = domain.Longitude
            };
        }
        
        public Location ConvertToDomain(LocationViewModel viewModel)
        {
            viewModel.ThrowIfNull("LocationViewModel", "viewModel object of type LocationViewModel can't be null");

            return new Location()
            {
                Address = _addressViewModelToDomainMapper.ConvertToDomain(viewModel.Address),
                Latitude = viewModel.Latitude,
                Longitude = viewModel.Longitude
            };
        }
    }
}