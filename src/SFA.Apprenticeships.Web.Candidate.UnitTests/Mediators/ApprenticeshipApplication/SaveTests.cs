namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using System;
    using Candidate.Mediators;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Candidate;
    using Candidate.ViewModels.VacancySearch;
    using Common.Constants;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SaveTests : TestsBase
    {
        private const int ValidVacancyId = 1;

        [Test]
        public void VacancyNotFound()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel()
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetOrCreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel { Status = ApplicationStatuses.ExpiredOrWithdrawn });
            
            var response = Mediator.Save(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertCode(Codes.ApprenticeshipApplication.Save.VacancyNotFound);
        }

        [Test]
        public void Error()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel()
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetOrCreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel("Has Error"));
            
            var response = Mediator.Save(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertMessage(Codes.ApprenticeshipApplication.Save.Error, ApplicationPageMessages.SaveFailed, UserMessageLevel.Warning, true);
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
                        FromYear = "1066"
                    }
                },
                VacancyDetail = new VacancyDetailViewModel(),
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetOrCreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel { Status = ApplicationStatuses.Draft });
            ApprenticeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<ApprenticeshipApplicationViewModel>(), It.IsAny<ApprenticeshipApplicationViewModel>())).Returns<Guid, ApprenticeshipApplicationViewModel, ApprenticeshipApplicationViewModel>((cid, svm, vm) => vm);

            var response = Mediator.Save(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertValidationResult(Codes.ApprenticeshipApplication.Save.ValidationError);
        }

        [Test]
        public void Ok()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel()
            };
            ApprenticeshipApplicationProvider.Setup(p => p.GetOrCreateApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel { Status = ApplicationStatuses.Draft });
            ApprenticeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<ApprenticeshipApplicationViewModel>(), It.IsAny<ApprenticeshipApplicationViewModel>())).Returns<Guid, ApprenticeshipApplicationViewModel, ApprenticeshipApplicationViewModel>((cid, svm, vm) => vm);
            ApprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            
            var response = Mediator.Save(Guid.NewGuid(), ValidVacancyId, viewModel);

            response.AssertCode(Codes.ApprenticeshipApplication.Save.Ok, true);
        }
    }
}