namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipApplication
{
    using Candidate.Mediators;
    using Candidate.Providers;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Moq;
    using NUnit.Framework;

    public abstract class TestsBase
    {
        protected Mock<ITraineeshipApplicationProvider> TraineeshipApplicationProvider;
        protected Mock<IConfigurationManager> ConfigurationManager;
        protected Mock<IUserDataProvider> UserDataProvider;
        protected ITraineeshipApplicationMediator Mediator;

        [SetUp]
        public void Setup()
        {
            TraineeshipApplicationProvider = new Mock<ITraineeshipApplicationProvider>();
            ConfigurationManager = new Mock<IConfigurationManager>();
            UserDataProvider = new Mock<IUserDataProvider>();
            Mediator = new TraineeshipApplicationMediator(TraineeshipApplicationProvider.Object, ConfigurationManager.Object, UserDataProvider.Object);
        }
    }
}