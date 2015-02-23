namespace SFA.Apprenticeships.Web.Employer.Mappers
{
    using System.Collections.Generic;
    using Domain.Entities;
    using Interfaces;
    using ViewModels;
    using SFA.Apprenticeships.Web.Employer.Extensions;

    public class EmployerEnquiryMapper : IDomainToViewModelMapper<EmployerEnquiry, EmployerEnquiryViewModel>, IViewModelToDomainMapper<EmployerEnquiryViewModel, EmployerEnquiry>
    {
        public EmployerEnquiryViewModel ConvertToViewModel(EmployerEnquiry domain)
        {
            domain.ThrowIfNull<Location>("Location", "Location can't be null");

            return new AddressViewModel()
            {
                AddressLine1 = domain.ApplicantAddress,
                AddressLine2 = domain.Email,
                AddressLine3 = domain.EmployeesCount,
                City = domain.EnquiryDescription,
                Country = domain.EnquirySource,
                County = domain.FirstName,
                Latitude = domain.Mobile,
                Longitude = domain.Position,
                Postcode = domain.PreviousExperienceType,
                Street = domain.SurName
            };
        }

        public IEnumerable<EmployerEnquiryViewModel> ConvertToViewModel(IEnumerable<EmployerEnquiry> domain)
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

        public EmployerEnquiry ConvertToDomain(EmployerEnquiryViewModel viewModel)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<EmployerEnquiry> ConvertToDomain(IEnumerable<EmployerEnquiryViewModel> viewModel)
        {
            throw new System.NotImplementedException();
        }
    }
}