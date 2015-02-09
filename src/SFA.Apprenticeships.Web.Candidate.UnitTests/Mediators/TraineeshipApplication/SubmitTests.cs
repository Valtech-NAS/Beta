namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipApplication
{
    using System;
    using Candidate.Mediators.Application;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Candidate;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Common.Models.Application;
    using Constants.Pages;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SubmitTests : TestsBase
    {
        private const int ValidVacancyId = 1;

        [Test]
        public void IncorrectState()
        {
            var viewModel = new TraineeshipApplicationViewModel
            {
                Candidate = new TraineeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel(),
                ViewModelStatus = ApplicationViewModelStatus.ApplicationInIncorrectState
            };
            TraineeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new TraineeshipApplicationViewModel { ViewModelStatus = ApplicationViewModelStatus.ApplicationInIncorrectState });
            TraineeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<TraineeshipApplicationViewModel>(), It.IsAny<TraineeshipApplicationViewModel>())).Returns<Guid, TraineeshipApplicationViewModel, TraineeshipApplicationViewModel>((cid, svm, vm) => vm);
            TraineeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<TraineeshipApplicationViewModel>())).Returns<Guid, int, TraineeshipApplicationViewModel>((cid, vid, vm) => vm);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertCode(TraineeshipApplicationMediatorCodes.Submit.IncorrectState, false);
        }

        [Test]
        public void Error()
        {
            var viewModel = new TraineeshipApplicationViewModel
            {
                Candidate = new TraineeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel(),
                ViewModelStatus = ApplicationViewModelStatus.Error
            };
            TraineeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new TraineeshipApplicationViewModel { ViewModelStatus = ApplicationViewModelStatus.Error });
            TraineeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<TraineeshipApplicationViewModel>(), It.IsAny<TraineeshipApplicationViewModel>())).Returns<Guid, TraineeshipApplicationViewModel, TraineeshipApplicationViewModel>((cid, svm, vm) => vm);
            TraineeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<TraineeshipApplicationViewModel>())).Returns<Guid, int, TraineeshipApplicationViewModel>((cid, vid, vm) => vm);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertMessage(TraineeshipApplicationMediatorCodes.Submit.Error, ApplicationPageMessages.SubmitApplicationFailed, UserMessageLevel.Warning, true, true);
        }

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

            response.AssertCode(TraineeshipApplicationMediatorCodes.Submit.Ok, false, true);
        }

        [Test]
        public void OkIsJavascript()
        {
            var viewModel = new TraineeshipApplicationViewModel
            {
                Candidate = new TraineeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel(),
                IsJavascript = true
            };
            TraineeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new TraineeshipApplicationViewModel());
            TraineeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<TraineeshipApplicationViewModel>(), It.IsAny<TraineeshipApplicationViewModel>())).Returns<Guid, TraineeshipApplicationViewModel, TraineeshipApplicationViewModel>((cid, svm, vm) => vm);
            TraineeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<TraineeshipApplicationViewModel>())).Returns<Guid, int, TraineeshipApplicationViewModel>((cid, vid, vm) => vm);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertCode(TraineeshipApplicationMediatorCodes.Submit.Ok, false, true);
        }
    }
}