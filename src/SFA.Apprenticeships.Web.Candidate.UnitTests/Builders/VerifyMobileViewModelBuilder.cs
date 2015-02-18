
namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using Candidate.ViewModels.Account;
    using Candidate.ViewModels.Applications;

    public class VerifyMobileViewModelBuilder
    {
        private string _mobileNumber;
        private string _mobileVerificationCode;
        private VerifyMobileState _verifyMobileState;
        private bool _showTraineeshipsLink;
        private bool _showTraineeshipsPrompt;

        public VerifyMobileViewModelBuilder PhoneNumber(string mobileNumber)
        {
            _mobileNumber = mobileNumber;
            return this;
        }

        public VerifyMobileViewModelBuilder VerifyMobileState(VerifyMobileState verifyMobileState)
        {
            _verifyMobileState = verifyMobileState;
            return this;
        }

        public VerifyMobileViewModelBuilder MobileVerificationCode(string mobileVerificationCode)
        {
            _mobileVerificationCode = mobileVerificationCode;
            return this;
        }

        public VerifyMobileViewModelBuilder ShowTraineeshipsLink(bool showTraineeshipsLink)
        {
            _showTraineeshipsLink = showTraineeshipsLink;
            return this;
        }

        public VerifyMobileViewModelBuilder ShowTraineeshipsPrompt(bool showTraineeshipsPrompt)
        {
            _showTraineeshipsPrompt = showTraineeshipsPrompt;
            return this;
        }

        public VerifyMobileViewModel Build()
        {
            var model = new VerifyMobileViewModel()
            {
                Status = _verifyMobileState,
                PhoneNumber = _mobileNumber,
                VerifyMobileCode = _mobileVerificationCode,
                TraineeshipFeature = new TraineeshipFeatureViewModel
                {
                    ShowTraineeshipsLink = _showTraineeshipsLink,
                    ShowTraineeshipsPrompt = _showTraineeshipsPrompt
                }
            };

            return model;
        }
    }
}
