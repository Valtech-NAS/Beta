namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipApplication
{
    using System;
    using Candidate.Mediators;
    using Candidate.Providers;
    using Candidate.ViewModels.Applications;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ApplyTests : TestsBase
    {
        private const int ValidVacancyId = 1;
        private const int InvalidVacancyId = 99999;

        [Test]
        public void HasError()
        {
            var mediator = GetMediator();

            var response = mediator.Apply(Guid.NewGuid(), InvalidVacancyId);

            response.AssertCode(Codes.TraineeshipApplication.Apply.HasError, false);
        }

        [Test]
        public void Ok()
        {
            var mediator = GetMediator();

            var response = mediator.Apply(Guid.NewGuid(), ValidVacancyId);

            response.AssertCode(Codes.TraineeshipApplication.Apply.Ok, true);
        }

        private static ITraineeshipApplicationMediator GetMediator()
        {
            var traineeshipApplicationProvider = new Mock<ITraineeshipApplicationProvider>();
            traineeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new TraineeshipApplicationViewModel());
            traineeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), InvalidVacancyId)).Returns(new TraineeshipApplicationViewModel("Vacancy not found"));
            var configurationManager = new Mock<IConfigurationManager>();
            var userDataProvider = new Mock<IUserDataProvider>();
            var mediator = GetMediator(traineeshipApplicationProvider.Object, configurationManager.Object, userDataProvider.Object);
            return mediator;
        }
    }
}