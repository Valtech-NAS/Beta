namespace SFA.Apprenticeships.Web.Employer.Providers.Interfaces
{
    using System.Collections.Generic;
    using Domain.Entities;
    using Domain.Enums;
    using ViewModels;

    public interface IEmployerEnquiryProvider
    {
        IEnumerable<AddressViewModel> FindAddress(string postcode);

        IEnumerable<ReferenceDataViewModel> GetReferenceData(ReferenceDataTypes type);

        void SubmitEnquiry(EmployerEnquiryViewModel message);
    }
}