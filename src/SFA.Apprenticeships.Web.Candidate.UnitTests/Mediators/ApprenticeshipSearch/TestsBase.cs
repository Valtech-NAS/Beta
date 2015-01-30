namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using Application.Interfaces.ReferenceData;
    using Candidate.Mediators;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Moq;
    using NUnit.Framework;

    public abstract class TestsBase
    {
        protected static IApprenticeshipSearchMediator GetMediator(IConfigurationManager configurationManager, ISearchProvider searchProvider, IApprenticeshipVacancyDetailProvider apprenticeshipVacancyDetailProvider, IUserDataProvider userDataProvider, IReferenceDataService referenceDataService)
        {
            return new ApprenticeshipSearchMediator(configurationManager, searchProvider, apprenticeshipVacancyDetailProvider, userDataProvider, referenceDataService, new ApprenticeshipSearchViewModelServerValidator(), new ApprenticeshipSearchViewModelLocationValidator());
        }

        protected Mock<IApprenticeshipVacancyDetailProvider> ApprenticeshipVacancyDetailProvider;
        protected Mock<IConfigurationManager> ConfigurationManager;
        protected Mock<IUserDataProvider> UserDataProvider;
        protected Mock<ISearchProvider> SearchProvider;
        protected Mock<IReferenceDataService> ReferenceDataService;
        protected IApprenticeshipSearchMediator Mediator;

        [SetUp]
        public virtual void Setup()
        {
            ApprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
            ConfigurationManager = new Mock<IConfigurationManager>();
            ConfigurationManager.Setup(cm => cm.GetAppSetting<int>("VacancyResultsPerPage")).Returns(5);
            ConfigurationManager.Setup(cm => cm.GetAppSetting("BlacklistedCategoryCodes")).Returns("00,99");
            UserDataProvider = new Mock<IUserDataProvider>();
            SearchProvider = new Mock<ISearchProvider>();
            ReferenceDataService = new Mock<IReferenceDataService>();
            Mediator = new ApprenticeshipSearchMediator(ConfigurationManager.Object, SearchProvider.Object, ApprenticeshipVacancyDetailProvider.Object, UserDataProvider.Object, ReferenceDataService.Object, new ApprenticeshipSearchViewModelServerValidator(), new ApprenticeshipSearchViewModelLocationValidator());
        }
    }
}