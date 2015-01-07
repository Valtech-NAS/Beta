namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
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

            var response = mediator.Submit(Guid.NewGuid(), ValidVacancyId);

            response.AssertCode(Codes.ApprenticeshipApplication.Apply.Ok, false, true);
        }

        private static IApprenticeshipApplicationMediator GetMediator()
        {
            var apprenticeshipApplicationProvider = new Mock<IApprenticeshipApplicationProvider>();
            apprenticeshipApplicationProvider.Setup(p => p.GetApplicationViewModel(It.IsAny<Guid>(), ValidVacancyId)).Returns(new ApprenticeshipApplicationViewModel());
            apprenticeshipApplicationProvider.Setup(p => p.PatchApplicationViewModel(It.IsAny<Guid>(), It.IsAny<ApprenticeshipApplicationViewModel>(), It.IsAny<ApprenticeshipApplicationViewModel>())).Returns<Guid, ApprenticeshipApplicationViewModel, ApprenticeshipApplicationViewModel>((cid, svm, vm) => vm);
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel()
            };
            apprenticeshipApplicationProvider.Setup(p => p.SubmitApplication(It.IsAny<Guid>(), It.IsAny<int>())).Returns(viewModel);
            var configurationManager = new Mock<IConfigurationManager>();
            var userDataProvider = new Mock<IUserDataProvider>();
            var mediator = GetMediator(apprenticeshipApplicationProvider.Object, configurationManager.Object, userDataProvider.Object);
            return mediator;
        }
    }
}