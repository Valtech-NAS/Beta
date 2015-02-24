namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using System;
    using Candidate.Mediators.Application;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Candidate;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Common.Models.Application;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SubmitTests : TestsBase
    {
        private const int ValidVacancyId = 1;

        [Test]
        public void VacancyNotFound()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel(),
                Status = ApplicationStatuses.ExpiredOrWithdrawn
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Submit.VacancyNotFound, false);
        }
        
        [Test]
        public void VacancyNotFound_GatewayError()
        {
            var getApplicationViewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel
                {
                    VacancyStatus = VacancyStatuses.Live
                }
            };
            var viewModel = new ApprenticeshipApplicationViewModel(ApprenticeshipVacancyDetailPageMessages.GetVacancyDetailFailed)
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel(ApprenticeshipVacancyDetailPageMessages.GetVacancyDetailFailed)
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(getApplicationViewModel);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId);

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.Submit.Error, ApplicationPageMessages.SubmitApplicationFailed, UserMessageLevel.Warning, false, true);
        }

        [Test]
        public void IncorrectState()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel
                {
                    VacancyStatus = VacancyStatuses.Live
                },
                ViewModelStatus = ApplicationViewModelStatus.ApplicationInIncorrectState
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Submit.IncorrectState, false);
        }

        [Test]
        public void GetApplicationViewModelError()
        {
            var getApplicationViewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel
                {
                    VacancyStatus = VacancyStatuses.Live
                }
            };
            var submitApplicationViewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel(),
                ViewModelStatus = ApplicationViewModelStatus.Error,
                ViewModelMessage = "An error message"
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(getApplicationViewModel);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(submitApplicationViewModel);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId);

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.Submit.Error, ApplicationPageMessages.SubmitApplicationFailed, UserMessageLevel.Warning, false, true);
        }

        [Test]
        public void SubmitApplicationError()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel(),
                ViewModelStatus = ApplicationViewModelStatus.Error,
                ViewModelMessage = "An error message"
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId);

            response.AssertMessage(ApprenticeshipApplicationMediatorCodes.Submit.Error, ApplicationPageMessages.SubmitApplicationFailed, UserMessageLevel.Warning, false, true);
        }

        [Test]
        public void ValidationError()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel
                {
                    Education = new EducationViewModel
                    {
                        NameOfMostRecentSchoolCollege = "A School",
                        FromYear = "0",
                        ToYear = "0"
                    }
                },
                VacancyDetail = new VacancyDetailViewModel
                {
                    VacancyStatus = VacancyStatuses.Live
                },
                ViewModelStatus = ApplicationViewModelStatus.Error
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId);

            response.AssertValidationResult(ApprenticeshipApplicationMediatorCodes.Submit.ValidationError);
        }

        [Test]
        public void Ok()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel
                {
                    VacancyStatus = VacancyStatuses.Live
                }
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            ApprenticeshipApplicationProvider.Setup(p => p.GetOrCreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel());
            ApprenticeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<ApprenticeshipApplicationViewModel>(), It.IsAny<ApprenticeshipApplicationViewModel>())).Returns<Guid, ApprenticeshipApplicationViewModel, ApprenticeshipApplicationViewModel>((cid, svm, vm) => vm);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            
            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Submit.Ok, false, true);
        }

        [Test]
        public void VacancyExpired()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel()
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            ApprenticeshipApplicationProvider.Setup(p => p.GetOrCreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel
            {
                VacancyDetail = new VacancyDetailViewModel
                {
                    VacancyStatus = VacancyStatuses.Expired
                }
            });
            ApprenticeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<ApprenticeshipApplicationViewModel>(), It.IsAny<ApprenticeshipApplicationViewModel>())).Returns<Guid, ApprenticeshipApplicationViewModel, ApprenticeshipApplicationViewModel>((cid, svm, vm) => vm);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);

            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Submit.VacancyNotFound, false);
        }

        [Test]
        public void ErrorGettingApplicationViewModel()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel()
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            ApprenticeshipApplicationProvider.Setup(p => p.GetOrCreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel(MyApplicationsPageMessages.ApplicationNotFound, ApplicationViewModelStatus.ApplicationNotFound));
            ApprenticeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<ApprenticeshipApplicationViewModel>(), It.IsAny<ApprenticeshipApplicationViewModel>())).Returns<Guid, ApprenticeshipApplicationViewModel, ApprenticeshipApplicationViewModel>((cid, svm, vm) => vm);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);

            var response = Mediator.Submit(Guid.NewGuid(), ValidVacancyId);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.Submit.VacancyNotFound, false);
        }
    }
}