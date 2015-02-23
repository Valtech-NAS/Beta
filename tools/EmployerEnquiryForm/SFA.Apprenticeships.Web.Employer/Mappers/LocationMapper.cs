namespace SFA.Apprenticeships.Web.Employer.Mappers
{
    using System.Collections.Generic;
    using Domain.Entities;
    using Interfaces;
    using ViewModels;
    using SFA.Apprenticeships.Web.Employer.Extensions;

    public class LocationMapper : IDomainToViewModelMapper<Location, AddressViewModel>, IViewModelToDomainMapper<AddressViewModel, Location>
    {
        public AddressViewModel ConvertToViewModel(Location domain)
        {
            domain.ThrowIfNull<Location>("Location", "Location can't be null");

            return new AddressViewModel()
            {
                AddressLine1 = domain.AddressLine1,
                AddressLine2 = domain.AddressLine2,
                AddressLine3 = domain.AddressLine3,
                City = domain.City,
                Country = domain.Country,
                County = domain.County,
                Latitude = domain.Latitude,
                Longitude = domain.Longitude,
                Postcode = domain.Postcode,
                Street = domain.Street
            };
        }
        
        public Location ConvertToDomain(AddressViewModel viewModel)
        {
            viewModel.ThrowIfNull<AddressViewModel>("AddressViewModel", "AddressViewModel can't be null");

            return new Location()
            {
                AddressLine1 = viewModel.AddressLine1,
                AddressLine2 = viewModel.AddressLine2,
                AddressLine3 = viewModel.AddressLine3,
                City = viewModel.City,
                Country = viewModel.Country,
                County = viewModel.County,
                Latitude = viewModel.Latitude,
                Longitude = viewModel.Longitude,
                Postcode = viewModel.Postcode,
                Street = viewModel.Street
            };
        }
    }
}