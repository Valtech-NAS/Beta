namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipApplication
{
    using System;
    using Candidate.Mediators;
    using Candidate.Providers;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Candidate;
    using Candidate.ViewModels.VacancySearch;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SubmitTests : TestsBase
    {
        private const int ValidVacancyId = 1;

        [Test]
        public void Ok()
        {
            var mediator = GetMediator();
            var viewModel = new TraineeshipApplicationViewModel
            {
                Candidate = new TraineeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel()
            };

            var response = mediator.Submit(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertParameters(Codes.TraineeshipApplication.Apply.Ok, false);
        }

        private static ITraineeshipApplicationMediator GetMediator()
        {
            var traineeshipApplicationProvider = new Mock<ITraineeshipApplicationProvider>();
            traineeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new TraineeshipApplicationViewModel());
            traineeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<TraineeshipApplicationViewModel>(), It.IsAny<TraineeshipApplicationViewModel>())).Returns<Guid, TraineeshipApplicationViewModel, TraineeshipApplicationViewModel>((cid, svm, vm) => vm);
            traineeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<TraineeshipApplicationViewModel>())).Returns<Guid, int, TraineeshipApplicationViewModel>((cid, vid, vm) => vm);
            var configurationManager = new Mock<IConfigurationManager>();
            var userDataProvider = new Mock<IUserDataProvider>();
            var mediator = GetMediator(traineeshipApplicationProvider.Object, configurationManager.Object, userDataProvider.Object);
            return mediator;
        }
    }
}