namespace SFA.Apprenticeships.Web.Employer.Mediators
{
    using System.Collections.Generic;
    using Domain.Enums;
    using Interfaces;
    using ViewModels;

    public class EmployerEnquiryMediator : IEmployerEnquiryMediator
    {
        public IEnumerable<AddressViewModel> FindAddress(string postcode)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ReferenceDataViewModel> GetReferenceData(ReferenceDataTypes type)
        {
            throw new System.NotImplementedException();
        }

        public void SubmitEnquiry(EmployerEnquiryViewModel message)
        {
            throw new System.NotImplementedException();
        }
    }
}