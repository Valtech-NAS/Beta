namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using Candidate.Mediators;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Moq;
    using NUnit.Framework;

    public abstract class TestsBase
    {
        protected Mock<IApprenticeshipApplicationProvider> ApprenticeshipApplicationProvider;
        protected Mock<IConfigurationManager> ConfigurationManager;
        protected Mock<IUserDataProvider> UserDataProvider;
        protected IApprenticeshipApplicationMediator Mediator;

        [SetUp]
        public void Setup()
        {
            ApprenticeshipApplicationProvider = new Mock<IApprenticeshipApplicationProvider>();
            ConfigurationManager = new Mock<IConfigurationManager>();
            UserDataProvider = new Mock<IUserDataProvider>();
            Mediator = new ApprenticeshipApplicationMediator(ApprenticeshipApplicationProvider.Object, new ApprenticeshipApplicationViewModelServerValidator(), new ApprenticeshipApplicationViewModelSaveValidator(), ConfigurationManager.Object, UserDataProvider.Object);
        }
    }
}