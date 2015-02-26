namespace SFA.Apprenticeships.Web.Employer.Mediators.Interfaces
{
    using System.Collections.Generic;
    using Domain.Enums;
    using ViewModels;

    public interface IEmployerEnquiryMediator
    {
        MediatorResponse<ReferenceDataListViewModel> GetReferenceData(ReferenceDataTypes type);

        MediatorResponse<EmployerEnquiryViewModel> SubmitEnquiry(EmployerEnquiryViewModel message);

        MediatorResponse<EmployerEnquiryViewModel> SubmitEnquiry();
    }
}