namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.AccountProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Builders;
    using Candidate.ViewModels.Account;
    using Domain.Entities.Exceptions;
    using Domain.Entities.UnitTests.Builder;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ResendTests
    {
        private const string PhoneNumber = "123456789";

        [TestCase(Domain.Entities.ErrorCodes.EntityStateError, VerifyMobileState.MobileVerificationNotRequired)]
        [TestCase(Application.Interfaces.Users.ErrorCodes.UnknownUserError, VerifyMobileState.Error)]
        public void GivenEntityStateError_ThenValidViewModelIsReturned(string errorCode, VerifyMobileState verifyMobileState)
        {
            //Arrange
            var candidateId = Guid.NewGuid();
            var candidateMock =
                new CandidateBuilder(candidateId).AllowMobile(true).PhoneNumber(PhoneNumber).VerifiedMobile(false).Build();

            var candidateServiceMock = new Mock<ICandidateService>();
            candidateServiceMock.Setup(cs => cs.GetCandidate(candidateId)).Returns(candidateMock);
            candidateServiceMock.Setup(cs => cs.SendMobileVerificationCode(candidateMock)).Throws(new CustomException(errorCode));

            var viewModel = new VerifyMobileViewModelBuilder().PhoneNumber(PhoneNumber).Build();
            var provider = new AccountProviderBuilder().With(candidateServiceMock).Build();

            //Act
            var result = provider.SendMobileVerificationCode(candidateId, viewModel);

            //Assert
            result.Status.Should().Be(verifyMobileState);
            result.HasError().Should().BeTrue();
            result.ViewModelMessage.Should().NotBeNull();
        }

        [Test]
        public void GivenValidCode_DefaultViewModelIsReturned()
        {
            //Arrange
            var candidateId = Guid.NewGuid();
            var candidateMock =
                new CandidateBuilder(candidateId).AllowMobile(true).PhoneNumber(PhoneNumber).VerifiedMobile(false).Build();

            var candidateServiceMock = new Mock<ICandidateService>();
            candidateServiceMock.Setup(cs => cs.GetCandidate(candidateId)).Returns(candidateMock);
            candidateServiceMock.Setup(cs => cs.SendMobileVerificationCode(candidateMock));

            var viewModel = new VerifyMobileViewModelBuilder().PhoneNumber(PhoneNumber).Build();
            var provider = new AccountProviderBuilder().With(candidateServiceMock).Build();

            //Act
            var result = provider.SendMobileVerificationCode(candidateId, viewModel);

            //Assert
            result.Status.Should().Be(VerifyMobileState.Ok);
            result.HasError().Should().BeFalse();
            result.ViewModelMessage.Should().BeNullOrEmpty();
        }
    }
}