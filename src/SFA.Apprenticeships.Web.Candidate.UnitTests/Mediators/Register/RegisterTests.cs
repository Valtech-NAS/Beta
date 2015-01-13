namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Register
{
    using System.Linq;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Constants.Pages;
    using Constants.ViewModels;
    using Candidate.Mediators;
    using Candidate.ViewModels.Register;
    using Common.Constants;

    [TestFixture]
    public class RegisterTests : RegisterBaseTests
    {
        private const string SomeName = "SomeName";
        private const string SomePhoneNumber = "0123456789";
        private const string ValidPassword = "?Password01!";
        private const string ValidEmailAddress = "kb@abc.com";

        [Test]
        public void IsUsernameAvailableFailedTest()
        {
            _candidateServiceProvider.Setup(x => x.IsUsernameAvailable(It.IsAny<string>()))
                .Returns(new UserNameAvailability {HasError = true});

            var registerViewModel = new RegisterViewModel {EmailAddress = ""};
            var response = _registerMediator.Register(registerViewModel);

            response.AssertMessage(Codes.RegisterMediatorCodes.Register.RegistrationFailed,
                RegisterPageMessages.RegistrationFailed, UserMessageLevel.Warning, true);
        }

        [Test]
        public void RegistrationFailedTest()
        {
            _candidateServiceProvider.Setup(x => x.IsUsernameAvailable(It.IsAny<string>()))
                .Returns(new UserNameAvailability {IsUserNameAvailable = true});

            _candidateServiceProvider.Setup(csp => csp.Register(It.IsAny<RegisterViewModel>())).Returns(false);

            var registerViewModel = new RegisterViewModel
            {
                EmailAddress = ValidEmailAddress,
                Password = ValidPassword,
                ConfirmPassword = ValidPassword,
                Firstname = SomeName,
                Lastname = SomeName,
                HasAcceptedTermsAndConditions = true,
                PhoneNumber = SomePhoneNumber
            };

            var response = _registerMediator.Register(registerViewModel);

            response.AssertMessage(Codes.RegisterMediatorCodes.Register.RegistrationFailed,
                RegisterPageMessages.RegistrationFailed, UserMessageLevel.Warning, true);
        }

        [Test]
        public void RegistrationValidationFailedAndUserNameInUseMessagePresentTest()
        {
            _candidateServiceProvider.Setup(x => x.IsUsernameAvailable(It.IsAny<string>()))
                .Returns(new UserNameAvailability {IsUserNameAvailable = false});

            var registerViewModel = new RegisterViewModel {EmailAddress = ValidEmailAddress};
            var response = _registerMediator.Register(registerViewModel);

            response.Code.Should().Be(Codes.RegisterMediatorCodes.Register.ValidationFailed);
            response.ViewModel.Should().Be(registerViewModel);
            response.Message.Should().BeNull();
            response.ValidationResult.IsValid.Should().BeFalse();
            response.ValidationResult.Errors.SingleOrDefault(
                e => e.ErrorMessage == RegisterViewModelMessages.EmailAddressMessages.UsernameNotAvailableErrorText)
                .Should()
                .NotBeNull();
        }

        [Test]
        public void RegistrationValidationFailedWhenEmailAddressIsNull()
        {
            _candidateServiceProvider.Setup(x => x.IsUsernameAvailable(It.IsAny<string>()))
                .Returns(new UserNameAvailability { HasError = true });

            var registerViewModel = new RegisterViewModel { EmailAddress = null };
            var response = _registerMediator.Register(registerViewModel);

            response.AssertValidationResult(Codes.RegisterMediatorCodes.Register.ValidationFailed, true);
        }

        [Test]
        public void SuccessfullyRegistered()
        {
            _candidateServiceProvider.Setup(x => x.IsUsernameAvailable(It.IsAny<string>()))
                .Returns(new UserNameAvailability {IsUserNameAvailable = true});

            _candidateServiceProvider.Setup(csp => csp.Register(It.IsAny<RegisterViewModel>())).Returns(true);

            var registerViewModel = new RegisterViewModel
            {
                EmailAddress = ValidEmailAddress,
                Password = ValidPassword,
                ConfirmPassword = ValidPassword,
                Firstname = SomeName,
                Lastname = SomeName,
                HasAcceptedTermsAndConditions = true,
                PhoneNumber = SomePhoneNumber
            };
            var response = _registerMediator.Register(registerViewModel);

            response.AssertCode(Codes.RegisterMediatorCodes.Register.SuccessfullyRegistered, true);
        }
    }
}