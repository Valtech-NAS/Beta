namespace SFA.Apprenticeships.Web.Employer.Mediators.Interfaces
{
    using System.Collections.Generic;
    using Domain.Enums;
    using ViewModels;

    public interface IEmployerEnquiryMediator
    {
        IEnumerable<AddressViewModel> FindAddress(string postcode);

        IEnumerable<ReferenceDataViewModel> GetReferenceData(ReferenceDataTypes type);

        void SubmitEnquiry(EmployerEnquiryViewModel message);
    }
}