namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class AcceptTermsAndConditionsTests
    {
        [TestCase("1")]
        [TestCase(null)]
        public void SuccessTest(string version)
        {
            var candidate = new Candidate
            {
                RegistrationDetails = new RegistrationDetails { AcceptedTermsAndConditionsVersion = version }
            };

            var candidateServiceProviderMock = new Mock<ICandidateServiceProvider>();
            candidateServiceProviderMock.Setup(x => x.GetCandidate(It.IsAny<Guid>())).Returns(candidate);
            var configurationManagerMock = new Mock<IConfigurationManager>();
            configurationManagerMock.Setup(x => x.GetAppSetting<string>("TermsAndConditionsVersion")).Returns("1.1");
            candidateServiceProviderMock.Setup(x => x.AcceptTermsAndConditions(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);
            var accountMediator = new AccountMediatorBuilder().With(candidateServiceProviderMock).With(configurationManagerMock).Build();

            var response = accountMediator.AcceptTermsAndConditions(Guid.NewGuid());

            response.Code.Should().Be(AccountMediatorCodes.AcceptTermsAndConditions.SuccessfullyAccepted);
        }

        [Test]
        public void AlreadyAcceptedTest()
        {
            var candidate = new Candidate
            {
                RegistrationDetails = new RegistrationDetails { AcceptedTermsAndConditionsVersion = "1.1" }
            };

            var candidateServiceProviderMock = new Mock<ICandidateServiceProvider>();
            candidateServiceProviderMock.Setup(x => x.GetCandidate(It.IsAny<Guid>())).Returns(candidate);
            var configurationManagerMock = new Mock<IConfigurationManager>();
            configurationManagerMock.Setup(x => x.GetAppSetting<string>("TermsAndConditionsVersion")).Returns("1.1");
            candidateServiceProviderMock.Setup(x => x.AcceptTermsAndConditions(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);
            var accountMediator = new AccountMediatorBuilder().With(candidateServiceProviderMock).With(configurationManagerMock).Build();

            var response = accountMediator.AcceptTermsAndConditions(Guid.NewGuid());

            response.Code.Should().Be(AccountMediatorCodes.AcceptTermsAndConditions.AlreadyAccepted);
        }

        [Test]
        public void ErrorTest()
        {
            var candidate = new Candidate
            {
                RegistrationDetails = new RegistrationDetails { AcceptedTermsAndConditionsVersion = "1" }
            };

            var candidateServiceProviderMock = new Mock<ICandidateServiceProvider>();
            candidateServiceProviderMock.Setup(x => x.GetCandidate(It.IsAny<Guid>())).Returns(candidate);
            var configurationManagerMock = new Mock<IConfigurationManager>();
            configurationManagerMock.Setup(x => x.GetAppSetting<string>("TermsAndConditionsVersion")).Returns("1.1");
            candidateServiceProviderMock.Setup(x => x.AcceptTermsAndConditions(It.IsAny<Guid>(), It.IsAny<string>())).Returns(false);
            var accountMediator = new AccountMediatorBuilder().With(candidateServiceProviderMock).With(configurationManagerMock).Build();

            var response = accountMediator.AcceptTermsAndConditions(Guid.NewGuid());

            response.Code.Should().Be(AccountMediatorCodes.AcceptTermsAndConditions.ErrorAccepting);
        }

        [Test]
        public void ExceptionTest()
        {
            var candidateServiceProviderMock = new Mock<ICandidateServiceProvider>();
            candidateServiceProviderMock.Setup(x => x.GetCandidate(It.IsAny<Guid>())).Throws<Exception>();
            var configurationManagerMock = new Mock<IConfigurationManager>();
            configurationManagerMock.Setup(x => x.GetAppSetting<string>("TermsAndConditionsVersion")).Returns("1.1");
            var accountMediator = new AccountMediatorBuilder().With(candidateServiceProviderMock).With(configurationManagerMock).Build();

            var response = accountMediator.AcceptTermsAndConditions(Guid.NewGuid());

            response.Code.Should().Be(AccountMediatorCodes.AcceptTermsAndConditions.ErrorAccepting);
        }
    }
}