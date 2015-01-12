namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipApplication
{
    using System;
    using Candidate.Mediators;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Candidate;
    using Candidate.ViewModels.VacancySearch;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SubmitTests : TestsBase
    {
        private const int ValidVacancyId = 1;

        [Test]
        public void Ok()
        {
            var viewModel = new TraineeshipApplicationViewModel
            {
                Candidate = new TraineeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel()
            };
            TraineeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new TraineeshipApplicationViewModel());
            TraineeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<TraineeshipApplicationViewModel>(), It.IsAny<TraineeshipApplicationViewModel>())).Returns<Guid, TraineeshipApplicationViewModel, TraineeshipApplicationViewModel>((cid, svm, vm) => vm);
            TraineeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<TraineeshipApplicationViewModel>())).Returns<Guid, int, TraineeshipApplicationViewModel>((cid, vid, vm) => vm);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertCode(Codes.TraineeshipApplication.Apply.Ok, false, true);
        }
    }
}