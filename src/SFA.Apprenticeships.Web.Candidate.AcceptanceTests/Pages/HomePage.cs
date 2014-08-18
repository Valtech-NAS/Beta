namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/")]
    [PageAlias("HomePage")]
    public class HomePage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomePage" /> class by using the provided parent control.
        /// </summary>
        /// <param name="context">The <see cref="ISearchContext" /> that contains this control.</param>
        public HomePage(ISearchContext context)
        {
        }

        /// <summary>
        ///     Gets or sets the students link button.
        /// </summary>
        /// <value>The students link button.</value>
        [ElementLocator(Class = "global-header__title")]
        public IWebElement Header { get; set; }

        [ElementLocator(Id = "vacancysearch")]
        public IWebElement VacancySearchLink { get; set; }
    }
}