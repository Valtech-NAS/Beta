namespace SFA.Apprenticeships.Web.Employer.Providers.Interfaces
{
    using System.Collections.Generic;
    using Domain.Entities;
    using Domain.Enums;
    using ViewModels;

    public interface IEmployerEnquiryProvider
    {
        ReferenceDataListViewModel GetReferenceData(ReferenceDataTypes type);

        SubmitQueryStatus SubmitEnquiry(EmployerEnquiryViewModel message);
    }
}