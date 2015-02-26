namespace SFA.Apprenticeships.Web.Employer.Mediators.EmployerEnquiry
{
    public class EmployerEnquiryMediatorCodes
    {
        public class FindAddress
        {
            public const string Success = "EmployerEnquiryMediator.FindAddress.Success";
            public const string ValidationError = "EmployerEnquiryMediator.FindAddress.ValidationError";
            public const string VerificationNotRequired = "EmployerEnquiryMediator.FindAddress.Failed";
            public const string Error = "EmployerEnquiryMediator.FindAddress.Error";
        }

        public class ReferenceData
        {
            public const string Success = "EmployerEnquiryMediator.ReferenceData.Success";
            public const string ValidationError = "EmployerEnquiryMediator.ReferenceData.ValidationError";
            public const string VerificationNotRequired = "EmployerEnquiryMediator.ReferenceData.Failed";
            public const string Error = "EmployerEnquiryMediator.ReferenceData.Error";
        }

        public class SubmitEnquiry
        {
            public const string Success = "EmployerEnquiryMediator.SubmitEnquiry.Success";
            public const string ValidationError = "EmployerEnquiryMediator.SubmitEnquiry.ValidationError";
            public const string VerificationNotRequired = "EmployerEnquiryMediator.SubmitEnquiry.Failed";
            public const string Error = "EmployerEnquiryMediator.SubmitEnquiry.Error";
        }
    }
}