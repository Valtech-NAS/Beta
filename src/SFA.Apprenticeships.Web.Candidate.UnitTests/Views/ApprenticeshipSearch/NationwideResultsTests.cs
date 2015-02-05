namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Candidate.ViewModels.VacancySearch;
    using Candidate.Views.ApprenticeshipSearch;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class NationwideResultsTests : MediatorTestsBase
    {
        [Test]
        public void LocalAndNationWideResultsInALocalSearch()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = ApprenticeshipLocationType.NonNational
                },
                TotalLocalHits = 2,
                TotalNationalHits = 3
            });

            view.GetElementbyId("result-message")
                    .InnerHtml.Should()
                    .Contain("We've found <b class=\"bold-medium\">2</b> apprenticeships in your selected area.");

            view.GetElementbyId("national-results-message")
                    .InnerHtml.Should()
                    .Contain("We've also found <a id='nationwideLocationTypeLink'")
                    .And.Contain("3 apprenticeships nationwide</a>.");
        }

        [Test]
        public void LocalAndNationWideResultsInANationwideSearch()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = ApprenticeshipLocationType.National
                },
                TotalLocalHits = 2,
                TotalNationalHits = 3
            });

            view.GetElementbyId("result-message")
                    .InnerHtml.Should()
                    .Contain("We've found <b class=\"bold-medium\">2</b> <a id='localLocationTypeLink' href=")
                    .And.Contain("apprenticeships in your selected area</a>.");

            view.GetElementbyId("national-results-message")
                    .InnerHtml.Should()
                    .Contain("We've also found 3 apprenticeships nationwide.");
        }

        [Test]
        public void OneLocalResultAndSomeNationWideResultsInALocalSearch()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = ApprenticeshipLocationType.NonNational
                },
                TotalLocalHits = 1,
                TotalNationalHits = 2
            });

            view.GetElementbyId("result-message")
                    .InnerHtml.Should()
                    .Contain("We've found <b class=\"bold-medium\">1</b> apprenticeship in your selected area.");

            view.GetElementbyId("national-results-message")
                    .InnerHtml.Should()
                    .Contain("We've also found <a id='nationwideLocationTypeLink'")
                    .And.Contain("2 apprenticeships nationwide</a>.");
        }

        [Test]
        public void OneLocalResultAndSomeNationWideResultsInANationwideSearch()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = ApprenticeshipLocationType.National
                },
                TotalLocalHits = 1,
                TotalNationalHits = 3
            });

            view.GetElementbyId("result-message")
                    .InnerHtml.Should()
                    .Contain("We've found <b class=\"bold-medium\">1</b> <a id='localLocationTypeLink' href=")
                    .And.Contain("apprenticeship in your selected area</a>.");

            view.GetElementbyId("national-results-message")
                    .InnerHtml.Should()
                    .Contain("We've also found 3 apprenticeships nationwide.");
        }

        [Test]
        public void NoLocalResultAndSomeNationWideResultsInALocalSearch()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = ApprenticeshipLocationType.NonNational
                },
                TotalLocalHits = 0,
                TotalNationalHits = 2
            });

            view.GetElementbyId("result-message")
                    .InnerHtml.Should().BeEmpty();

            view.GetElementbyId("national-results-message")
                    .InnerHtml.Should()
                    .Contain("We've found <a id='nationwideLocationTypeLink'")
                    .And.Contain("2 apprenticeships nationwide</a>.");
        }

        [Test]
        public void NoLocalResultAndSomeNationWideResultsInANationwideSearch()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = ApprenticeshipLocationType.National
                },
                TotalLocalHits = 0,
                TotalNationalHits = 3
            });

            view.GetElementbyId("result-message")
                    .InnerHtml.Should().BeEmpty();

            view.GetElementbyId("national-results-message")
                    .InnerHtml.Should()
                    .Contain("We've found 3 apprenticeships nationwide.");
        }

        [Test]
        public void SomeLocalResultsAndOneNationWideResultsInALocalSearch()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = ApprenticeshipLocationType.NonNational
                },
                TotalLocalHits = 2,
                TotalNationalHits = 1
            });

            view.GetElementbyId("result-message")
                    .InnerHtml.Should()
                    .Contain("We've found <b class=\"bold-medium\">2</b> apprenticeships in your selected area.");

            view.GetElementbyId("national-results-message")
                    .InnerHtml.Should()
                    .Contain("We've also found <a id='nationwideLocationTypeLink'")
                    .And.Contain("1 apprenticeship nationwide</a>.");
        }

        [Test]
        public void SomeLocalResultsAndOneNationWideResultInANationwideSearch()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = ApprenticeshipLocationType.National
                },
                TotalLocalHits = 3,
                TotalNationalHits = 1
            });

            view.GetElementbyId("result-message")
                    .InnerHtml.Should()
                    .Contain("We've found <b class=\"bold-medium\">3</b> <a id='localLocationTypeLink' href=")
                    .And.Contain("apprenticeships in your selected area</a>.");

            view.GetElementbyId("national-results-message")
                    .InnerHtml.Should()
                    .Contain("We've also found 1 apprenticeship nationwide.");
        }

        [Test]
        public void SomeLocalResultsAndNoNationWideResultsInALocalSearch()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = ApprenticeshipLocationType.NonNational
                },
                TotalLocalHits = 2,
                TotalNationalHits = 0
            });

            view.GetElementbyId("result-message")
                    .InnerHtml.Should()
                    .Contain("We've found <b class=\"bold-medium\">2</b> apprenticeships in your selected area.");

            view.GetElementbyId("national-results-message")
                    .InnerHtml.Should().BeEmpty();
        }

        [Test]
        public void SomeLocalResultAndNoNationWideResultsInANationwideSearch()
        {
            var results = new Results();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = ApprenticeshipLocationType.National
                },
                TotalLocalHits = 3,
                TotalNationalHits = 0
            });

            view.GetElementbyId("result-message")
                    .InnerHtml.Should()
                    .Contain("We've found <b class=\"bold-medium\">3</b> <a id='localLocationTypeLink' href=")
                    .And.Contain("apprenticeships in your selected area</a>.");

            view.GetElementbyId("national-results-message")
                    .InnerHtml.Should().BeEmpty();
        }

        [Test]
        public void NationWideResultsCanNotHaveTheirSortOrderChanged()
        {
            var results = new searchResults();

            var view = results.RenderAsHtml(new ApprenticeshipSearchResponseViewModel
            {
                VacancySearch = new ApprenticeshipSearchViewModel
                {
                    LocationType = ApprenticeshipLocationType.National,
                    SortTypes = new SelectList(new List<string> { "one", "two"})
                },
                TotalLocalHits = 2,
                TotalNationalHits = 3
            });

            view.GetElementbyId("sort-results").Attributes["disabled"].Value.Should().Be("disabled");
        }
    }
}