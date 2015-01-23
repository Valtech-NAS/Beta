namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Register
{
    using System.Linq;
    using Candidate.ViewModels;
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

            var registerViewModel = new RegisterViewModel
            {
                Firstname = SomeName,
                Lastname = SomeName,
                EmailAddress = ValidEmailAddress,
                PhoneNumber = SomePhoneNumber,
                Password = ValidPassword,
                ConfirmPassword = ValidPassword,
                HasAcceptedTermsAndConditions = true
            };
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

        [TestCase("email@domain.com", true)]
        [TestCase("firstname.lastname@domain.com", true)]
        [TestCase("email@subdomain.domain.com", true)]
        [TestCase("firstname+lastname@domain.com", true)]
        // [TestCase("email@123.123.123.123", true)] The regex doesn't support this scenario
        // [TestCase("email@[123.123.123.123]", true)] The regex doesn't support this scenario
        [TestCase("\"email\"@domain.com", true)]
        [TestCase("1234567890@domain.com", true)]
        [TestCase("email@domain-one.com", true)]
        [TestCase("_______@domain.com", true)]
        [TestCase("email@domain.name", true)]
        [TestCase("email@domain.co.jp", true)]
        [TestCase("emailAddress@gmail.com", true)]
        [TestCase("firstname-lastname@domain.com", true)]
        [TestCase("plainaddress", false)]
        [TestCase("#@%^%#$@#$@#.com", false)]
        // [TestCase("@domain.com", false)] The regex doesn't support this scenario
        [TestCase("Joe Smith <email@domain.com>", false)]
        [TestCase("email.domain.com", false)]
        [TestCase("email@domain@domain.com", false)]
        [TestCase(".email@domain.com", false)]
        [TestCase("email.@domain.com", false)]
        [TestCase("email..email@domain.com", false)]
        [TestCase("あいうえお@domain.com", false)]
        [TestCase("email@domain.com (Joe Smith)", false)]
        [TestCase("email@domain", false)]
        [TestCase("email@-domain.com", false)]
        // [TestCase("email@domain.web", false)] The regex doesn't support this scenario
        [TestCase("email@111.222.333.44444", false)]
        [TestCase("email@domain..com", false)]
        public void ValidateEmail(string email, bool isValid)
        {
            _candidateServiceProvider.Setup(x => x.IsUsernameAvailable(It.IsAny<string>()))
                .Returns(new UserNameAvailability { IsUserNameAvailable = true });

            _candidateServiceProvider.Setup(csp => csp.Register(It.IsAny<RegisterViewModel>())).Returns(true);

            var registerViewModel = new RegisterViewModel
            {
                EmailAddress = email,
                Password = ValidPassword,
                ConfirmPassword = ValidPassword,
                Firstname = SomeName,
                Lastname = SomeName,
                HasAcceptedTermsAndConditions = true,
                PhoneNumber = SomePhoneNumber
            };
            var response = _registerMediator.Register(registerViewModel);

            Assert.AreEqual(isValid, response.Code == "SuccessfullyRegistered");
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