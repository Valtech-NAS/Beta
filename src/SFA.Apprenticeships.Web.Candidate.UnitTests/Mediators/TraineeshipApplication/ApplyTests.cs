namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipApplication
{
    using System;
    using Candidate.Mediators;
    using Candidate.ViewModels.Applications;
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
            TraineeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), InvalidVacancyId)).Returns(new TraineeshipApplicationViewModel("Vacancy not found"));
            
            var response = Mediator.Apply(Guid.NewGuid(), InvalidVacancyId);

            response.AssertCode(Codes.TraineeshipApplication.Apply.HasError, false);
        }

        [Test]
        public void Ok()
        {
            TraineeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new TraineeshipApplicationViewModel());
            
            var response = Mediator.Apply(Guid.NewGuid(), ValidVacancyId);

            response.AssertCode(Codes.TraineeshipApplication.Apply.Ok, true);
        }
    }
}