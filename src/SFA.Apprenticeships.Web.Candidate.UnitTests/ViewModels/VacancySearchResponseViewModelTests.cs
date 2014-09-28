namespace SFA.Apprenticeships.Web.Candidate.UnitTests.ViewModels
{
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using Candidate.ViewModels.VacancySearch;

    //todo: refactor to be a more general set of "paging" tests as it's nothing specifically to do with vacancy searches
    [TestFixture]
    public class VacancySearchResponseViewModelTests
    {
        [TestCase(100, 10, 10)]
        [TestCase(100, 0, 1)]
        [TestCase(10, 100, 1)]
        [TestCase(105, 10, 11)]
        public void ShouldReturnTheNumberOfPagesGivenThePageSize(int hits, int pageSize, int expected)
        {
            var test = new VacancySearchResponseViewModel
            {
                VacancySearch = new VacancySearchViewModel {LocationType = VacancyLocationType.NonNational},
                TotalLocalHits = hits,
                PageSize = pageSize
            };

            test.Pages.Should().Be(expected);
        }

        [TestCase(1, 2)]
        [TestCase(11, 11)]
        public void ShouldReturnNextPageNUmberGivenStartPage(int startPage, int expected)
        {
            var test = new VacancySearchResponseViewModel
            {
                TotalLocalHits = 101,
                PageSize = 10,
                VacancySearch = new VacancySearchViewModel { PageNumber = startPage, LocationType = VacancyLocationType.NonNational }
            };

            test.NextPage.Should().Be(expected);
        }

        [TestCase()]
        public void ShouldReturnNextPageIfNoStartPage()
        {
            var test = new VacancySearchResponseViewModel
            {
                TotalLocalHits = 101,
                PageSize = 10
            };
            test.NextPage.Should().Be(1);
        }

        [TestCase(1, 0)]
        [TestCase(11, 10)]
        public void ShouldReturnPrevPageNUmberGivenStartPage(int startPage, int expected)
        {
            var test = new VacancySearchResponseViewModel { TotalLocalHits = 101, PageSize = 10, VacancySearch = new VacancySearchViewModel { PageNumber = startPage, LocationType = VacancyLocationType.NonNational } };

            test.PrevPage.Should().Be(expected);
        }

        [TestCase()]
        public void ShouldReturnPrevPageIfNoStartPage()
        {
            var test = new VacancySearchResponseViewModel
            {
                TotalLocalHits = 101,
                PageSize = 10,
                VacancySearch = new VacancySearchViewModel {LocationType = VacancyLocationType.NonNational}
            };
            test.PrevPage.Should().Be(1);
        }
    }
}
