namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipApplication
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
            var viewModel = new TraineeshipApplicationViewModel
            {
                Candidate = new TraineeshipCandidateViewModel(),
                VacancyDetail = new VacancyDetailViewModel()
            };

            var response = mediator.AddEmptyWorkExperienceRows(viewModel);

            response.AssertCode(Codes.TraineeshipApplication.AddEmptyWorkExperienceRows.Ok, true);
        }

        private static ITraineeshipApplicationMediator GetMediator()
        {
            var traineeshipApplicationProvider = new Mock<ITraineeshipApplicationProvider>();
            var configurationManager = new Mock<IConfigurationManager>();
            var userDataProvider = new Mock<IUserDataProvider>();
            var mediator = GetMediator(traineeshipApplicationProvider.Object, configurationManager.Object, userDataProvider.Object);
            return mediator;
        }
    }
}