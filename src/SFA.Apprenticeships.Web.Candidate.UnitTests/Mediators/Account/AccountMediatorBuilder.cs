using Moq;
using SFA.Apprenticeships.Domain.Interfaces.Configuration;
using SFA.Apprenticeships.Web.Candidate.Mediators.Account;
using SFA.Apprenticeships.Web.Candidate.Providers;
using SFA.Apprenticeships.Web.Candidate.Validators;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using Common.Providers;

    internal class AccountMediatorBuilder
    {
        private Mock<IApprenticeshipApplicationProvider> _apprenticeshipApplicationProviderMock = new Mock<IApprenticeshipApplicationProvider>();
        private Mock<IApprenticeshipVacancyDetailProvider> _apprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
        private Mock<ITraineeshipVacancyDetailProvider> _traineeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
        private IAccountProvider _accountProvider = new Mock<IAccountProvider>().Object;
        private Mock<ICandidateServiceProvider> _candidateServiceProviderMock = new Mock<ICandidateServiceProvider>();
        private Mock<IConfigurationManager> _configurationManagerMock = new Mock<IConfigurationManager>();
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

        public AccountMediatorBuilder With(IAccountProvider accountProvider)
        {
            _accountProvider = accountProvider;
            return this;
        }

        public AccountMediatorBuilder With(Mock<ICandidateServiceProvider> candidateServiceProvider)
        {
            _candidateServiceProviderMock = candidateServiceProvider;
            return this;
        }

        public AccountMediatorBuilder With(Mock<IConfigurationManager> configurationManager)
        {
            _configurationManagerMock = configurationManager;
            return this;
        }

        public AccountMediator Build()
        {
            return new AccountMediator(_accountProvider,
                _candidateServiceProviderMock.Object,
                _settingsViewModelServerValidator,
                _apprenticeshipApplicationProviderMock.Object,
                _apprenticeshipVacancyDetailProvider.Object,
                _traineeshipVacancyDetailProvider.Object,
                _configurationManagerMock.Object);
        }
    }
}
