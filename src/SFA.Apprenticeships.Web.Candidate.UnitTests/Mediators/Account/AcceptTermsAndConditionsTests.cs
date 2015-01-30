namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using System;
    using Candidate.Mediators;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.Validators;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class AcceptTermsAndConditionsTests
    {
        private AccountMediator _accountMediator;
        private Mock<IApprenticeshipApplicationProvider> _apprenticeshipApplicationProviderMock;
        private Mock<IApprenticeshipVacancyDetailProvider> _apprenticeshipVacancyDetailProvider;
        private Mock<ITraineeshipVacancyDetailProvider> _traineeshipVacancyDetailProvider;
        private Mock<IAccountProvider> _accountProviderMock;
        private Mock<ICandidateServiceProvider> _candidateServiceProviderMock;
        private Mock<IConfigurationManager> _configurationManagerMock;
        private SettingsViewModelServerValidator _settingsViewModelServerValidator;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _apprenticeshipApplicationProviderMock = new Mock<IApprenticeshipApplicationProvider>();
            _apprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
            _traineeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();

            _accountProviderMock = new Mock<IAccountProvider>();
            _settingsViewModelServerValidator = new SettingsViewModelServerValidator();
            _candidateServiceProviderMock = new Mock<ICandidateServiceProvider>();
            _configurationManagerMock = new Mock<IConfigurationManager>();

            _accountMediator = new AccountMediator(
                _accountProviderMock.Object,
                _candidateServiceProviderMock.Object,
                _settingsViewModelServerValidator,
                _apprenticeshipApplicationProviderMock.Object,
                _apprenticeshipVacancyDetailProvider.Object,
                _traineeshipVacancyDetailProvider.Object,
                _configurationManagerMock.Object);
        }

        [TestCase("1")]
        [TestCase(null)]
        public void SuccessTest(string version)
        {
            var candidate = new Candidate
            {
                RegistrationDetails = new RegistrationDetails { AcceptedTermsAndConditionsVersion = version }
            };
            _candidateServiceProviderMock.Setup(x => x.GetCandidate(It.IsAny<Guid>())).Returns(candidate);
            _configurationManagerMock.Setup(x => x.GetAppSetting<string>("TermsAndConditionsVersion")).Returns("1.1");
            _candidateServiceProviderMock.Setup(x => x.AcceptTermsAndConditions(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);
            var response = _accountMediator.AcceptTermsAndConditions(Guid.NewGuid());

            response.Code.Should().Be(Codes.AccountMediator.AcceptTermsAndConditions.SuccessfullyAccepted);
        }

        [Test]
        public void AlreadyAcceptedTest()
        {
            var candidate = new Candidate
            {
                RegistrationDetails = new RegistrationDetails { AcceptedTermsAndConditionsVersion = "1.1" }
            };

            _candidateServiceProviderMock.Setup(x => x.GetCandidate(It.IsAny<Guid>())).Returns(candidate);
            _configurationManagerMock.Setup(x => x.GetAppSetting<string>("TermsAndConditionsVersion")).Returns("1.1");
            _candidateServiceProviderMock.Setup(x => x.AcceptTermsAndConditions(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);

            var response = _accountMediator.AcceptTermsAndConditions(Guid.NewGuid());

            response.Code.Should().Be(Codes.AccountMediator.AcceptTermsAndConditions.AlreadyAccepted);
        }

        [Test]
        public void ErrorTest()
        {
            var candidate = new Candidate
            {
                RegistrationDetails = new RegistrationDetails { AcceptedTermsAndConditionsVersion = "1" }
            };

            _candidateServiceProviderMock.Setup(x => x.GetCandidate(It.IsAny<Guid>())).Returns(candidate);
            _configurationManagerMock.Setup(x => x.GetAppSetting<string>("TermsAndConditionsVersion")).Returns("1.1");
            _candidateServiceProviderMock.Setup(x => x.AcceptTermsAndConditions(It.IsAny<Guid>(), It.IsAny<string>())).Returns(false);

            var response = _accountMediator.AcceptTermsAndConditions(Guid.NewGuid());

            response.Code.Should().Be(Codes.AccountMediator.AcceptTermsAndConditions.ErrorAccepting);
        }

        [Test]
        public void ExceptionTest()
        {
            _candidateServiceProviderMock.Setup(x => x.GetCandidate(It.IsAny<Guid>())).Throws<Exception>();
            _configurationManagerMock.Setup(x => x.GetAppSetting<string>("TermsAndConditionsVersion")).Returns("1.1");
            var response = _accountMediator.AcceptTermsAndConditions(Guid.NewGuid());

            response.Code.Should().Be(Codes.AccountMediator.AcceptTermsAndConditions.ErrorAccepting);
        }
    }
}