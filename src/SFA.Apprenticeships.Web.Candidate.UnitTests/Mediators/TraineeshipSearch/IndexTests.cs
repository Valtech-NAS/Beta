namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipSearch
{
    using System.Linq;
    using Application.Interfaces.Vacancies;
    using Candidate.Mediators;
    using Candidate.Mediators.Traineeships;
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

            response.Code.Should().Be(Codes.TraineeshipSearch.Index.Ok);
            response.ViewModel.Should().NotBeNull();
            response.Message.Should().BeNull();
            response.Parameters.Should().BeNull();
            response.ValidationResult.Should().BeNull();

            var viewModel = response.ViewModel;

            viewModel.WithinDistance.Should().Be(40);
            viewModel.ResultsPerPage.Should().Be(5);
            viewModel.SortType.Should().Be(VacancySortType.Distance);

            viewModel.Distances.Should().NotBeNull();
            viewModel.Distances.SelectedValue.Should().Be(2);

            viewModel.SortTypes.Should().NotBeNull();
            viewModel.SortTypes.Count().Should().BeGreaterThan(0);
            viewModel.SortTypes.SelectedValue.Should().Be(VacancySortType.Distance);
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