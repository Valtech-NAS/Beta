namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using System;
    using Candidate.Mediators;
    using Candidate.Providers;
    using Candidate.ViewModels.Applications;
    using Common.Constants;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ResumeTests : TestsBase
    {
        private const int ValidVacancyId = 1;
        private const int InvalidVacancyId = 99999;

        [Test]
        public void HasError()
        {
            var mediator = GetMediator();

            var response = mediator.Resume(Guid.NewGuid(), InvalidVacancyId);

            response.AssertMessage(Codes.ApprenticeshipApplication.Resume.HasError, "Vacancy not found", UserMessageLevel.Warning, false);
        }

        [Test]
        public void Ok()
        {
            var mediator = GetMediator();

            var response = mediator.Resume(Guid.NewGuid(), ValidVacancyId);

            response.AssertCode(Codes.ApprenticeshipApplication.Resume.Ok, false, true);
        }

        private static IApprenticeshipApplicationMediator GetMediator()
        {
            var apprenticeshipApplicationProvider = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProvider.Setup(p => p.GetOrCreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel());
            apprenticeshipApplicationProvider.Setup(p => p.GetOrCreateApplicationViewModel(It.IsAny<Guid>(), InvalidVacancyId)).Returns(new ApprenticeshipApplicationViewModel("Vacancy not found"));
            var configurationManager = new Mock<IConfigurationManager>();
            var userDataProvider = new Mock<IUserDataProvider>();
            var mediator = GetMediator(apprenticeshipApplicationProvider.Object, configurationManager.Object, userDataProvider.Object);
            return mediator;
        }
    }
}