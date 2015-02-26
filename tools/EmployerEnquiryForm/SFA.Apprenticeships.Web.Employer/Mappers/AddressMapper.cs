namespace SFA.Apprenticeships.Web.Employer.Mappers
{
    using Common.Extensions;
    using Domain.Entities;
    using Interfaces;
    using ViewModels;

    public class AddressMapper : IDomainToViewModelMapper<Address, AddressViewModel>, IViewModelToDomainMapper<AddressViewModel, Address>
    {
        public AddressViewModel ConvertToViewModel(Address domain)
        {
            domain.ThrowIfNull("Address", "domain object of type Address can't be null");

            return new AddressViewModel()
            {
                AddressLine1 = domain.AddressLine1,
                AddressLine2 = domain.AddressLine2,
                AddressLine3 = domain.AddressLine3,
                City = domain.City,
                Postcode = domain.Postcode
            };
        }

        public Address ConvertToDomain(AddressViewModel viewModel)
        {
            viewModel.ThrowIfNull("AddressViewModel", "viewModel object of type AddressViewModel can't be null");

            return new Address()
            {
                AddressLine1 = viewModel.AddressLine1,
                AddressLine2 = viewModel.AddressLine2,
                AddressLine3 = viewModel.AddressLine3,
                City = viewModel.City,
                Postcode = viewModel.Postcode
            };
        }
    }
}