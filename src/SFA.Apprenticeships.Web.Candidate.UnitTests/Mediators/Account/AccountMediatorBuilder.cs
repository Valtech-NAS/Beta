using Moq;
using SFA.Apprenticeships.Domain.Interfaces.Configuration;
using SFA.Apprenticeships.Web.Candidate.Mediators.Account;
using SFA.Apprenticeships.Web.Candidate.Providers;
using SFA.Apprenticeships.Web.Candidate.Validators;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    internal class AccountMediatorBuilder
    {
        private Mock<IApprenticeshipApplicationProvider> _apprenticeshipApplicationProviderMock = new Mock<IApprenticeshipApplicationProvider>();
        private Mock<IApprenticeshipVacancyDetailProvider> _apprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
        private Mock<ITraineeshipVacancyDetailProvider> _traineeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
        private Mock<IAccountProvider> _accountProviderMock = new Mock<IAccountProvider>();
        private Mock<ICandidateServiceProvider> _candidateServiceProviderMock = new Mock<ICandidateServiceProvider>();
        private readonly Mock<IConfigurationManager> _configurationManagerMock = new Mock<IConfigurationManager>();
        private readonly SettingsViewModelServerValidator _settingsViewModelServerValidator = new SettingsViewModelServerValidator();
        
        public AccountMediatorBuilder With(Mock<IApprenticeshipVacancyDetailProvider> apprenticeshipVacancyDetailProvider)
        {
            _apprenticeshipVacancyDetailProvider = apprenticeshipVacancyDetailProvider;
            return this;
        }

        public AccountMediatorBuilder With(Mock<IApprenticeshipApplicationProvider> apprenticeshipApplicationProvider)
        {
            _apprenticeshipApplicationProviderMock = apprenticeshipApplicationProvider;
            return this;
        }

        public AccountMediatorBuilder With(Mock<ITraineeshipVacancyDetailProvider> traineeshipVacancyDetailProvider)
        {
            _traineeshipVacancyDetailProvider = traineeshipVacancyDetailProvider;
            return this;
        }

        public AccountMediatorBuilder With(Mock<IAccountProvider> accountProvider)
        {
            _accountProviderMock = accountProvider;
            return this;
        }

        public AccountMediatorBuilder With(Mock<ICandidateServiceProvider> candidateServiceProvider)
        {
            _candidateServiceProviderMock = candidateServiceProvider;
            return this;
        }

        public AccountMediator Build()
        {
            return new AccountMediator(_accountProviderMock.Object,
                _candidateServiceProviderMock.Object,
                _settingsViewModelServerValidator,
                _apprenticeshipApplicationProviderMock.Object,
                _apprenticeshipVacancyDetailProvider.Object,
                _traineeshipVacancyDetailProvider.Object,
                _configurationManagerMock.Object);
        }
    }

}
