namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.AccountProvider
{
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Logging;
    using Candidate.Mappers;
    using Candidate.Providers;
    using Configuration;
    using Moq;

    public class AccountProviderBuilder
    {
        private Mock<ICandidateService> _candidateService;
        private readonly Mock<ILogService> _logger;
        private Mock<IFeatureToggle> _featureToggle;
        
        public AccountProviderBuilder()
        {
            _candidateService = new Mock<ICandidateService>();
            _logger = new Mock<ILogService>();
            _featureToggle = new Mock<IFeatureToggle>();
        }

        public AccountProviderBuilder With(Mock<ICandidateService> candidateService)
        {
            _candidateService = candidateService;
            return this;
        }

        public AccountProviderBuilder With(Mock<IFeatureToggle> featureToggle)
        {
            _featureToggle = featureToggle;
            return this;
        }

        public AccountProvider Build()
        {
            var provider = new AccountProvider(_candidateService.Object, new ApprenticeshipCandidateWebMappers(), _logger.Object, _featureToggle.Object);
            return provider;
        }
    }
}