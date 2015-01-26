namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Extensions
{
    using Candidate.Extensions;
    using Candidate.ViewModels.VacancySearch;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class CollectionExtensionsTests
    {
        [Test]
        public void SubCategoriesTest()
        {
            var searchViewModel = new ApprenticeshipSearchViewModel
            {
                SubCategories = new[]
                {
                    "1_1",
                    "1_2",
                    "2_1"
                }
            };

            var queryString = searchViewModel.SubCategories.ToQueryString("SubCategories");

            queryString.Should().Be("&SubCategories=1_1&SubCategories=1_2&SubCategories=2_1");
        }
    }
}