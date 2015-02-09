namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApplicationProvider
{
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Logging;
    using Candidate.Mappers;
    using Candidate.Providers;
    using Domain.Interfaces.Configuration;
    using Moq;
    using NUnit.Framework;

    public abstract class ApprenticeshipApplicationProviderTestsBase
    {
        protected const int ValidVacancyId = 1;
        protected const int InvalidVacancyId = 999999;

        protected Mock<IApprenticeshipVacancyDetailProvider> ApprenticeshipVacancyDetailProvider;
        protected Mock<ICandidateService> CandidateService;
        protected Mock<IConfigurationManager> ConfigurationManager;
        protected Mock<ILogService> LogService;
        protected ApprenticeshipApplicationProvider ApprenticeshipApplicationProvider;

        [SetUp]
        public virtual void SetUp()
        {
            ApprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
            CandidateService = new Mock<ICandidateService>();
            ConfigurationManager = new Mock<IConfigurationManager>();
            LogService = new Mock<ILogService>();

            ApprenticeshipApplicationProvider = new ApprenticeshipApplicationProvider(ApprenticeshipVacancyDetailProvider.Object, CandidateService.Object, new ApprenticeshipCandidateWebMappers(), ConfigurationManager.Object, LogService.Object);
        }
    }
}