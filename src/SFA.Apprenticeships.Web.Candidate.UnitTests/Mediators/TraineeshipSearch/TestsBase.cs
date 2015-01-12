namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipSearch
{
    using Candidate.Mediators.Traineeships;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Moq;
    using NUnit.Framework;

    public abstract class TestsBase
    {
        protected static ITraineeshipSearchMediator GetMediator(IConfigurationManager configurationManager, ISearchProvider searchProvider, ITraineeshipVacancyDetailProvider traineeshipVacancyDetailProvider, IUserDataProvider userDataProvider)
        {
            return new TraineeshipSearchMediator(configurationManager, searchProvider, traineeshipVacancyDetailProvider, userDataProvider, new TraineeshipSearchViewModelServerValidator(), new TraineeshipSearchViewModelLocationValidator());
        }

        protected Mock<ITraineeshipVacancyDetailProvider> TraineeshipVacancyDetailProvider;
        protected Mock<IConfigurationManager> ConfigurationManager;
        protected Mock<IUserDataProvider> UserDataProvider;
        protected Mock<ISearchProvider> SearchProvider;
        protected ITraineeshipSearchMediator Mediator;

        [SetUp]
        public virtual void Setup()
        {
            TraineeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
            ConfigurationManager = new Mock<IConfigurationManager>();
            UserDataProvider = new Mock<IUserDataProvider>();
            SearchProvider = new Mock<ISearchProvider>();
            Mediator = new TraineeshipSearchMediator(ConfigurationManager.Object, SearchProvider.Object, TraineeshipVacancyDetailProvider.Object, UserDataProvider.Object, new TraineeshipSearchViewModelServerValidator(), new TraineeshipSearchViewModelLocationValidator());
        }
    }
}