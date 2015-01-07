namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
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
    public class AddEmptyWorkExperienceRowsTests : TestsBase
    {
        [Test]
        public void Ok()
        {
            var mediator = GetMediator();
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel()
            };

            var response = mediator.AddEmptyWorkExperienceRows(viewModel);

            response.AssertCode(Codes.ApprenticeshipApplication.AddEmptyWorkExperienceRows.Ok, true);
        }

        private static IApprenticeshipApplicationMediator GetMediator()
        {
            var apprenticeshipApplicationProvider = new Mock<IApprenticeshipApplicationProvider>();
            var configurationManager = new Mock<IConfigurationManager>();
            var userDataProvider = new Mock<IUserDataProvider>();
            var mediator = GetMediator(apprenticeshipApplicationProvider.Object, configurationManager.Object, userDataProvider.Object);
            return mediator;
        }
    }
}