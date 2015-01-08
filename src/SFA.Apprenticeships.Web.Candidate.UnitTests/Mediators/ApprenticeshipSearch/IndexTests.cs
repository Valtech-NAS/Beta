namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using Candidate.Mediators;
    using Candidate.Providers;
    using Common.Constants;
    using Common.Providers;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class IndexTests : TestsBase
    {
        private Mock<IConfigurationManager> _configurationManager;
        private Mock<ISearchProvider> _searchProvider;
        private Mock<IApprenticeshipVacancyDetailProvider> _apprenticeshipVacancyDetailProvider;
        private Mock<IUserDataProvider> _userDataProvider;
        private IApprenticeshipSearchMediator _mediator;

        [SetUp]
        public void Setup()
        {
            _configurationManager = new Mock<IConfigurationManager>();
            _configurationManager.Setup(cm => cm.GetAppSetting<int>("VacancyResultsPerPage")).Returns(5);
            _searchProvider = new Mock<ISearchProvider>();
            _apprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
            _userDataProvider = new Mock<IUserDataProvider>();
            _mediator = GetMediator(_configurationManager.Object, _searchProvider.Object, _apprenticeshipVacancyDetailProvider.Object, _userDataProvider.Object);
        }

        [Test]
        public void Ok()
        {
            var response = _mediator.Index();

            response.AssertCode(Codes.ApprenticeshipSearch.Index.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.WithinDistance.Should().Be(2);
            viewModel.LocationType.Should().Be(ApprenticeshipLocationType.NonNational);
            viewModel.ResultsPerPage.Should().Be(5);
            viewModel.ApprenticeshipLevels.Should().NotBeNull();
            viewModel.ApprenticeshipLevel.Should().Be("All");
        }

        [Test]
        public void RememberApprenticeshipLevel()
        {
            _userDataProvider.Setup(udp => udp.Get(UserDataItemNames.ApprenticeshipLevel)).Returns("Advanced");

            var response = _mediator.Index();

            var viewModel = response.ViewModel;
            viewModel.ApprenticeshipLevel.Should().Be("Advanced");
        }
    }
}