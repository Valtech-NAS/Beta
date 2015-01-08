namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Register
{
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using Candidate.Mediators;
    using Candidate.ViewModels.Register;
    using Common.Constants;
    using Constants.Pages;
    using Constants.ViewModels;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class RegisterTests : RegisterBaseTests
    {
        [Test]
        public void RegistrationFailedTest()
        {
            _candidateServiceProvider.Setup(x => x.IsUsernameAvailable(It.IsAny<string>()))
                .Returns(new UserNameAvailability() {HasError = true});

            var registerViewModel = new RegisterViewModel {EmailAddress = ""};
            var response = _registerMediator.Register(registerViewModel);

            response.Code.Should().Be(Codes.RegisterMediatorCodes.Register.RegistrationFailed);
            response.ViewModel.Should().Be(registerViewModel);
            response.Message.Text.Should().Be(RegisterPageMessages.RegistrationFailed);
            response.Message.Level.Should().Be(UserMessageLevel.Warning);
        }

        [Test]
        public void RegistrationValidationFailedAndUserNameInUseMessagePresentTest()
        {
            _candidateServiceProvider.Setup(x => x.IsUsernameAvailable(It.IsAny<string>()))
                .Returns(new UserNameAvailability() { IsUserNameAvailable = false });

            var registerViewModel = new RegisterViewModel { EmailAddress = "kb@abc.com" };
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
    }
}
