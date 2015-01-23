namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipSearch
{
    using Candidate.Mediators;
    using Common.Constants;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class IndexTests : TestsBase
    {
        [Test]
        public void Ok()
        {
            var response = Mediator.Index();

            response.AssertCode(Codes.ApprenticeshipSearch.Index.Ok, true);

            var viewModel = response.ViewModel;
            viewModel.WithinDistance.Should().Be(5);
            viewModel.LocationType.Should().Be(ApprenticeshipLocationType.NonNational);
            viewModel.ResultsPerPage.Should().Be(5);
            viewModel.Distances.SelectedValue.Should().Be(null);
            viewModel.ApprenticeshipLevels.Should().NotBeNull();
            viewModel.ApprenticeshipLevel.Should().Be("All");
        }

        [Test]
        public void RememberApprenticeshipLevel()
        {
            UserDataProvider.Setup(udp => udp.Get(UserDataItemNames.ApprenticeshipLevel)).Returns("Advanced");

            var response = Mediator.Index();

            var viewModel = response.ViewModel;
            viewModel.ApprenticeshipLevel.Should().Be("Advanced");
        }
    }
}