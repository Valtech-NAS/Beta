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
            domain.ThrowIfNull<Location>("EmployerEnquiry", "EmployerEnquiry can't be null");

            return new EmployerEnquiryViewModel();
            {
            };
        }

        public EmployerEnquiry ConvertToDomain(EmployerEnquiryViewModel viewModel)
        {
            viewModel.ThrowIfNull<Location>("EmployerEnquiryViewModel", "EmployerEnquiryViewModel can't be null");
            
            return new EmployerEnquiry();
        }
    }
}