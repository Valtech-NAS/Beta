namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipSearch
{
    using System.Linq;
    using Application.Interfaces.Vacancies;
    using Candidate.Mediators.Search;
    using Candidate.Providers;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class IndexTests : TestsBase
    {
        [Test]
        public void Ok()
        {
            var mediator = GetMediator();
            var response = mediator.Index();

            response.AssertCode(TraineeshipSearchMediatorCodes.Index.Ok, true);

            var viewModel = response.ViewModel;

            viewModel.WithinDistance.Should().Be(40);
            viewModel.ResultsPerPage.Should().Be(5);
            viewModel.SortType.Should().Be(VacancySearchSortType.Distance);

            viewModel.Distances.Should().NotBeNull();
            viewModel.Distances.SelectedValue.Should().Be(null);

            viewModel.SortTypes.Should().NotBeNull();
            viewModel.SortTypes.Count().Should().BeGreaterThan(0);
            viewModel.SortTypes.SelectedValue.Should().Be(VacancySearchSortType.Distance);
        }

        private static ITraineeshipSearchMediator GetMediator()
        {
            var configurationManager = new Mock<IConfigurationManager>();

            configurationManager.Setup(cm => cm.GetAppSetting<int>("VacancyResultsPerPage")).Returns(5);

            var searchProvider = new Mock<ISearchProvider>();
            var traineeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
            var userDataProvider = new Mock<IUserDataProvider>();

            return GetMediator(configurationManager.Object, searchProvider.Object, traineeshipVacancyDetailProvider.Object, userDataProvider.Object);
        }
    }
}