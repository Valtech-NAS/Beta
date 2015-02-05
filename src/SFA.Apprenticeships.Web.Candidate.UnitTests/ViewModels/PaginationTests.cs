namespace SFA.Apprenticeships.Web.Candidate.UnitTests.ViewModels
{
    using Candidate.ViewModels.VacancySearch;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class PaginationTests
    {
        [TestCase(100, 10, 10)]
        [TestCase(100, 0, 1)]
        [TestCase(10, 100, 1)]
        [TestCase(105, 10, 11)]
        public void ShouldReturnTheNumberOfPagesGivenThePageSize(int hits, int pageSize, int expected)
        {
            var test = new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel {LocationType = ApprenticeshipLocationType.NonNational},
                TotalLocalHits = hits,
                PageSize = pageSize
            };

            test.Pages.Should().Be(expected);
        }

        [TestCase(1, 2)]
        [TestCase(11, 11)]
        public void ShouldReturnNextPageNumberGivenStartPage(int startPage, int expected)
        {
            var test = new ApprenticeshipSearchResponseViewModel
            {
                TotalLocalHits = 101,
                PageSize = 10,
                VacancySearch = new ApprenticeshipSearchViewModel { PageNumber = startPage, LocationType = ApprenticeshipLocationType.NonNational }
            };

            test.NextPage.Should().Be(expected);
        }

        [Test]
        public void ShouldReturnNextPageIfNoStartPage()
        {
            var test = new ApprenticeshipSearchResponseViewModel
            {
                TotalLocalHits = 101,
                PageSize = 10
            };
            test.NextPage.Should().Be(1);
        }

        [TestCase(1, 0)]
        [TestCase(11, 10)]
        public void ShouldReturnPrevPageNumberGivenStartPage(int startPage, int expected)
        {
            var test = new ApprenticeshipSearchResponseViewModel { TotalLocalHits = 101, PageSize = 10, VacancySearch = new ApprenticeshipSearchViewModel { PageNumber = startPage, LocationType = ApprenticeshipLocationType.NonNational } };

            test.PrevPage.Should().Be(expected);
        }

        [Test]
        public void ShouldReturnPrevPageIfNoStartPage()
        {
            var test = new ApprenticeshipSearchResponseViewModel
            {
                TotalLocalHits = 101,
                PageSize = 10,
                VacancySearch = new ApprenticeshipSearchViewModel {LocationType = ApprenticeshipLocationType.NonNational}
            };
            test.PrevPage.Should().Be(0);
        }
    }
}
