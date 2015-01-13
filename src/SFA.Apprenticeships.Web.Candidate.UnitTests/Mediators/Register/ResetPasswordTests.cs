namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Register
{
    using Moq;
    using NUnit.Framework;
    using SFA.Apprenticeships.Domain.Entities.Users;
    using SFA.Apprenticeships.Web.Candidate.Constants.Pages;
    using SFA.Apprenticeships.Web.Candidate.Mediators;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Register;
    using SFA.Apprenticeships.Web.Common.Constants;

    [TestFixture]
    public class ResetPasswordTests : RegisterBaseTests
    {
        private const string ValidPassword = "?Password01!";
        private const string ValidPasswordResetCode = "ABC123";
        private const string VaildEmailAddress = "validEmailAddress@gmail.com";
        private const string InvalidPasswordResetCode = "invalidPasswordResetCode";
        private const string ErrorMessage = "Some error message";

        [Test]
        public void ValidationFailure()
        {
            var resetPasswordViewModel = new PasswordResetViewModel
            {
                PasswordResetCode = InvalidPasswordResetCode
            };

            var response = _registerMediator.ResetPassword(resetPasswordViewModel);

            response.AssertValidationResult(Codes.RegisterMediatorCodes.ResetPassword.FailedValidation, true);
        }

        [Test]
        public void ErrorVerifyingPassword()
        {
            var resetPasswordViewModel = GetValidPasswordResetViewModel();

            _candidateServiceProvider.Setup(csp => csp.VerifyPasswordReset(It.IsAny<PasswordResetViewModel>()))
                .Returns(new PasswordResetViewModel
                {
                    ViewModelMessage = ErrorMessage
                });

            var response = _registerMediator.ResetPassword(resetPasswordViewModel);

            response.AssertMessage(Codes.RegisterMediatorCodes.ResetPassword.FailedToResetPassword, ErrorMessage, UserMessageLevel.Warning, true);
        }

        [Test]
        public void UserLocked()
        {
            var resetPasswordViewModel = GetValidPasswordResetViewModel();

            _candidateServiceProvider.Setup(csp => csp.VerifyPasswordReset(It.IsAny<PasswordResetViewModel>()))
                .Returns(new PasswordResetViewModel
                {
                    UserStatus = UserStatuses.Locked
                });

            var response = _registerMediator.ResetPassword(resetPasswordViewModel);

            response.AssertCode(Codes.RegisterMediatorCodes.ResetPassword.UserAccountLocked, true);
        }

        [Test]
        public void InvalidResetCode()
        {
            var resetPasswordViewModel = GetValidPasswordResetViewModel();

            _candidateServiceProvider.Setup(csp => csp.VerifyPasswordReset(It.IsAny<PasswordResetViewModel>()))
                .Returns(new PasswordResetViewModel
                {
                    IsPasswordResetCodeValid = false
                });

            var response = _registerMediator.ResetPassword(resetPasswordViewModel);

            response.AssertValidationResult(Codes.RegisterMediatorCodes.ResetPassword.InvalidResetCode);
        }

        [Test]
        public void SuccessfullResetPassword()
        {
            var resetPasswordViewModel = GetValidPasswordResetViewModel();

            _candidateServiceProvider.Setup(csp => csp.VerifyPasswordReset(It.IsAny<PasswordResetViewModel>()))
                .Returns(new PasswordResetViewModel
                {
                    IsPasswordResetCodeValid = true
                });

            var response = _registerMediator.ResetPassword(resetPasswordViewModel);

            response.AssertMessage(Codes.RegisterMediatorCodes.ResetPassword.SuccessfullyResetPassword,PasswordResetPageMessages.SuccessfulPasswordReset, UserMessageLevel.Success, true);
        }

        private static PasswordResetViewModel GetValidPasswordResetViewModel()
        {
            var resetPasswordViewModel = new PasswordResetViewModel
            {
                PasswordResetCode = ValidPasswordResetCode,
                EmailAddress = VaildEmailAddress,
                Password = ValidPassword,
                ConfirmPassword = ValidPassword
            };
            return resetPasswordViewModel;
        }
    }
}
