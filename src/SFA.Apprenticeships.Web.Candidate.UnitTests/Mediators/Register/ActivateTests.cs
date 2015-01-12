namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Register
{
    using System;
    using Moq;
    using NUnit.Framework;
    using SFA.Apprenticeships.Web.Candidate.Constants.Pages;
    using SFA.Apprenticeships.Web.Candidate.Mediators;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Register;
    using SFA.Apprenticeships.Web.Common.Constants;

    [TestFixture]
    public class ActivateTests : RegisterBaseTests
    {
        private const string SomeErrorMessage = "SomeErrorMessage";
        private const string ValidActivationCode = "ABC123";
        private const string ValidEmailAddress = "validEmailAddress@gmail.com";
        private const string InvalidActivationCode = "invalidActivationCode";

        [Test]
        public void ValidationError()
        {
            var activationViewModel = new ActivationViewModel
            {
                ActivationCode = InvalidActivationCode
            };
            var response = _registerMediator.Activate(Guid.NewGuid(), activationViewModel);

            response.AssertValidationResult(Codes.RegisterMediatorCodes.Activate.FailedValidation, true);
        }

        [Test]
        public void UserActivated()
        {
            var activationViewModel = new ActivationViewModel
            {
                ActivationCode = ValidActivationCode,
                EmailAddress = ValidEmailAddress
            };

            _candidateServiceProvider.Setup(csp => csp.Activate(It.IsAny<ActivationViewModel>(), It.IsAny<Guid>()))
                .Returns(new ActivationViewModel
                {
                    State = ActivateUserState.Activated
                });

            var response = _registerMediator.Activate(Guid.NewGuid(), activationViewModel);

            response.AssertMessage(Codes.RegisterMediatorCodes.Activate.SuccessfullyActivated, ActivationPageMessages.AccountActivated, UserMessageLevel.Success, true);
        }

        [Test]
        public void ActivationError()
        {
            var activationViewModel = new ActivationViewModel
            {
                ActivationCode = ValidActivationCode,
                EmailAddress = ValidEmailAddress
            };

            _candidateServiceProvider.Setup(csp => csp.Activate(It.IsAny<ActivationViewModel>(), It.IsAny<Guid>()))
                .Returns(new ActivationViewModel
                {
                    State = ActivateUserState.Error,
                    ViewModelMessage = SomeErrorMessage
                });

            var response = _registerMediator.Activate(Guid.NewGuid(), activationViewModel);

            response.AssertMessage(Codes.RegisterMediatorCodes.Activate.SuccessfullyActivated, SomeErrorMessage, UserMessageLevel.Success, true);
        }

        [Test]
        public void InvalidCode()
        {
            var activationViewModel = new ActivationViewModel
            {
                ActivationCode = ValidActivationCode,
                EmailAddress = ValidEmailAddress
            };

            _candidateServiceProvider.Setup(csp => csp.Activate(It.IsAny<ActivationViewModel>(), It.IsAny<Guid>()))
                .Returns(new ActivationViewModel
                {
                    State = ActivateUserState.InvalidCode
                });

            var response = _registerMediator.Activate(Guid.NewGuid(), activationViewModel);

            response.AssertValidationResult(Codes.RegisterMediatorCodes.Activate.InvalidActivationCode, true);
        }
    }
}