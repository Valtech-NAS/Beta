using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Bindings
{
    using NUnit.Framework;
    using OpenQA.Selenium;
    using Pages;
    using TechTalk.SpecFlow;

    [Binding]
    public class SearchResults
    {
        private readonly ISearchContext _context;
        private readonly VacancySearchResultPage _vacancySearchResultPage;

        public SearchResults(ISearchContext context)
        {
            _context = context;
        }

        [Then(@"I see search results")]
        public void ThenISeeSearchResults()
        {
            Assert.Equals(5, _vacancySearchResultPage.SearchResults.Count());
        }

    }
}
