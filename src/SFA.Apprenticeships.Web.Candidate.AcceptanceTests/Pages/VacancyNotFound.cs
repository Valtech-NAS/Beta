namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;

    [PageNavigation("/VacancyNotFound.html")]
    [PageAlias("VacancyNotFound")]
    public class VacancyNotFound
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomePage" /> class by using the provided parent control.
        /// </summary>
        /// <param name="context">The <see cref="ISearchContext" /> that contains this control.</param>
        public VacancyNotFound(ISearchContext context)
        {
        }
    }
}