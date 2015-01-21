namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.ApprenticeshipSearch
{
    using OpenQA.Selenium;
    using SpecBind.Pages;

    [PageNavigation("/apprenticeship/[0-9]+")]
    [PageAlias("Error")]
    public class ApprenticeshipVacancyNotFound
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomePage" /> class by using the provided parent control.
        /// </summary>
        /// <param name="context">The <see cref="ISearchContext" /> that contains this control.</param>
        public ApprenticeshipVacancyNotFound(ISearchContext context)
        {
        }

        [ElementLocator(Class = "heading-xlarge")]
        public IWebElement Heading { get; set; }
    }
}