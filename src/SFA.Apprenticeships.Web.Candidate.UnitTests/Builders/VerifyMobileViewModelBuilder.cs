
namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using Candidate.ViewModels.Account;
    using Candidate.ViewModels.Login;

    public class VerifyMobileViewModelBuilder
    {
        private readonly string _mobileNumber;
        private readonly string _mobileVerificationCode;
        private readonly VerifyMobileState _verifyMobileState;

        public VerifyMobileViewModelBuilder(string mobileNumber, string mobileVerificationCode, VerifyMobileState verifyMobileState)
        {
            _mobileNumber = mobileNumber;
            _mobileVerificationCode = mobileVerificationCode;
            _verifyMobileState = verifyMobileState;
        }

        public VerifyMobileViewModelBuilder(string mobileNumber, VerifyMobileState verifyMobileState)
        {
            _mobileNumber = mobileNumber;
            _verifyMobileState = verifyMobileState;
        }
        
        public VerifyMobileViewModelBuilder(string mobileNumber)
        {
            _mobileNumber = mobileNumber;
        }

        public VerifyMobileViewModel Build()
        {
            var model = new VerifyMobileViewModel()
            {
                Status = _verifyMobileState,
                MobileNumber = _mobileNumber,
                VerifyMobileCode = _mobileVerificationCode
            };

            return model;
        }
    }
}
