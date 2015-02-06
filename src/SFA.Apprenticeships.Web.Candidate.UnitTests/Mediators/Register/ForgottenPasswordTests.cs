namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Register
{
    using Moq;
    using NUnit.Framework;
    using SFA.Apprenticeships.Web.Candidate.Constants.Pages;
    using SFA.Apprenticeships.Web.Candidate.Mediators;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Register;
    using SFA.Apprenticeships.Web.Common.Constants;

    [TestFixture]
    public class ForgottenPasswordTests : RegisterBaseTests
    {
        private const string InvalidEmailAddress = "invalidEmailAddress";
        private const string ValidEmailAddress = "ValidEmailAddress@gmail.com";

        [Test]
        public void ValidationErrors()
        {
            var forgottenPasswordViewModel = new ForgottenPasswordViewModel
            {
                EmailAddress = InvalidEmailAddress
            };

            var response = _registerMediator.ForgottenPassword(forgottenPasswordViewModel);

            response.AssertValidationResult(Codes.RegisterMediatorCodes.ForgottenPassword.FailedValidation, true);
        }

        [Test]
        public void PasswordSent()
        {
            var forgottenPasswordViewModel = new ForgottenPasswordViewModel
            {
                EmailAddress = ValidEmailAddress
            };

            _candidateServiceProvider.Setup(
                csp => csp.RequestForgottenPasswordResetCode(It.IsAny<ForgottenPasswordViewModel>())).Returns(true);

            var response = _registerMediator.ForgottenPassword(forgottenPasswordViewModel);

            response.AssertCode(Codes.RegisterMediatorCodes.ForgottenPassword.PasswordSent, true);
        }

        [Test]
        public void PasswordNotSent()
        {
            var forgottenPasswordViewModel = new ForgottenPasswordViewModel
            {
                EmailAddress = ValidEmailAddress
            };

            _candidateServiceProvider.Setup(
                csp => csp.RequestForgottenPasswordResetCode(It.IsAny<ForgottenPasswordViewModel>())).Returns(false);

            var response = _registerMediator.ForgottenPassword(forgottenPasswordViewModel);

            response.AssertMessage(Codes.RegisterMediatorCodes.ForgottenPassword.FailedToSendResetCode,
                PasswordResetPageMessages.FailedToSendPasswordResetCode, UserMessageLevel.Warning, true);
        }
    }
}
