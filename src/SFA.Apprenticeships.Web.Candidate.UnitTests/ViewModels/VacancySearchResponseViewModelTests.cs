
namespace SFA.Apprenticeships.Web.Candidate.UnitTests.ViewModels
{
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;

    [TestFixture]
    public class VacancySearchResponseViewModelTests
    {
        [TestCase(100, 10, 10)]
        [TestCase(100, 0, 1)]
        [TestCase(10, 100, 1)]
        [TestCase(105, 10, 11)]
        public void ShouldReturnTheNumberOfPagesGivenThePageSize(int hits, int pageSize, int expected)
        {
            var test = new VacancySearchResponseViewModel {TotalHits = hits};

            test.Pages(pageSize).Should().Be(expected);
        }
    }
}
