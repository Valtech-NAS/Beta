namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/")]
    [PageAlias("HomePage")]
    public class HomePage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HomePage" /> class by using the provided parent control.
        /// </summary>
        /// <param name="parent">The <see cref="UITestControl" /> that contains this control.</param>
        public HomePage(ISearchContext test)
        {
        }

        /// <summary>
        ///     Gets or sets the students link button.
        /// </summary>
        /// <value>The students link button.</value>
        [ElementLocator(Class = "global-header__title")]
        public IWebElement Header { get; set; }
    }
}