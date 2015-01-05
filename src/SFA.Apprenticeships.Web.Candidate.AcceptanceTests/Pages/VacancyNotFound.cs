namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/apprenticeship/[0-9]+")]
    [PageAlias("Error")]
    public class VacancyNotFound
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomePage" /> class by using the provided parent control.
        /// </summary>
        /// <param name="context">The <see cref="ISearchContext" /> that contains this control.</param>
        public VacancyNotFound(ISearchContext context)
        {
        }

        [ElementLocator(Class = "heading-xlarge")]
        public IWebElement Heading { get; set; }
    }
}