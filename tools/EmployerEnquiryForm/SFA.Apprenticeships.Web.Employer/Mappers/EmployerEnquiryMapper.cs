namespace SFA.Apprenticeships.Web.Employer.Mappers
{
    using Common.Extensions;
    using Domain.Entities;
    using Interfaces;
    using ViewModels;

    public class EmployerEnquiryMapper : IDomainToViewModelMapper<EmployerEnquiry, EmployerEnquiryViewModel>, IViewModelToDomainMapper<EmployerEnquiryViewModel, EmployerEnquiry>
    {
        private IDomainToViewModelMapper<Address, AddressViewModel> _addressDomainToViewModelMapper;
        private IViewModelToDomainMapper<AddressViewModel, Address> _addressViewModelToDomainMapper;

        public EmployerEnquiryMapper(IDomainToViewModelMapper<Address, AddressViewModel> addressDomainToViewModelMapper,
                                     IViewModelToDomainMapper<AddressViewModel, Address> addressViewModelToDomainMapper)
        {
            _addressDomainToViewModelMapper = addressDomainToViewModelMapper;
            _addressViewModelToDomainMapper = addressViewModelToDomainMapper;
        }

        public EmployerEnquiryViewModel ConvertToViewModel(EmployerEnquiry domain)
        {
            domain.ThrowIfNull("EmployerEnquiry", "domain object of type EmployerEnquiry can't be null");

            return new EmployerEnquiryViewModel()
            {
                Address = _addressDomainToViewModelMapper.ConvertToViewModel(domain.ApplicantAddress),
                Email = domain.Email,
                EmployeesCount = domain.EmployeesCount,
                EnquiryDescription = domain.EnquiryDescription,
                EnquirySource = domain.EnquirySource,
                Firstname = domain.Firstname,
                Lastname = domain.Lastname,
                WorkPhoneNumber = domain.WorkPhoneNumber,
                MobileNumber = domain.MobileNumber,
                Title = domain.Title,
                Companyname= domain.Companyname,
                Position = domain.Position,
                PreviousExperienceType = domain.PreviousExperienceType,
                WorkSector = domain.WorkSector
            };
        }

        public EmployerEnquiry ConvertToDomain(EmployerEnquiryViewModel viewModel)
        {
            viewModel.ThrowIfNull("EmployerEnquiryViewModel", "viewModel object of type EmployerEnquiryViewModel can't be null");

            return new EmployerEnquiry()
            {
                ApplicantAddress = _addressViewModelToDomainMapper.ConvertToDomain(viewModel.Address),
                Email = viewModel.Email,
                EmployeesCount = viewModel.EmployeesCount,
                EnquiryDescription = viewModel.EnquiryDescription,
                EnquirySource = viewModel.EnquirySource,
                Firstname = viewModel.Firstname,
                Lastname = viewModel.Lastname,
                Companyname = viewModel.Companyname,
                WorkPhoneNumber = viewModel.WorkPhoneNumber,
                MobileNumber = viewModel.MobileNumber,
                Title = viewModel.Title,
                Position = viewModel.Position,
                PreviousExperienceType = viewModel.PreviousExperienceType,
                WorkSector = viewModel.WorkSector
            };
        }
    }
}