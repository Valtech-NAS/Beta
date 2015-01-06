namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipSearch
{
    using System.Web.UI.WebControls;
    using Application.Interfaces.Vacancies;
    using Candidate.Mediators;
    using Candidate.Mediators.Traineeships;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class IndexTests
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
            viewModel.Distances.SelectedValue.Should().Be(2); // TODO: AG: MEDIATORS: testing select lists from base class.
            viewModel.SortTypes.Should().NotBeNull(); // TODO: AG: MEDIATORS: testing select lists from base class.
        }

        private static ITraineeshipSearchMediator GetMediator()
        {
            var configurationManager = new Mock<IConfigurationManager>();

            configurationManager.Setup(cm => cm.GetAppSetting<int>("VacancyResultsPerPage")).Returns(5);

            var searchProvider = new Mock<ISearchProvider>();
            var traineeshipVacancyDetailProvider = new Mock<ITraineeshipVacancyDetailProvider>();
            var userDataProvider = new Mock<IUserDataProvider>();
            var mediator = GetMediator(configurationManager.Object, searchProvider.Object, traineeshipVacancyDetailProvider.Object, userDataProvider.Object);

            return mediator;
        }

        private static ITraineeshipSearchMediator GetMediator(IConfigurationManager configurationManager, ISearchProvider searchProvider, ITraineeshipVacancyDetailProvider traineeshipVacancyDetailProvider, IUserDataProvider userDataProvider)
        {
            return new TraineeshipSearchMediator(
                configurationManager, searchProvider, traineeshipVacancyDetailProvider, userDataProvider, new TraineeshipSearchViewModelClientValidator(), new TraineeshipSearchViewModelLocationValidator());
        }
    }
}