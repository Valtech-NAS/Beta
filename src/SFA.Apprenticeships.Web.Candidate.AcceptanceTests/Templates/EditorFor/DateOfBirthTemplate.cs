namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Templates.EditorFor
{
    using global::SpecBind.Pages;
    using global::SpecBind.Selenium;
    using OpenQA.Selenium;

    [ElementLocator(Class = "date-input")]
    public class DateOfBirthTemplate : WebElement
    {
        public DateOfBirthTemplate(ISearchContext searchContext) : base(searchContext)
        {
        }

        [ElementLocator(Id = "DateOfBirth_Day")]
        public IWebElement Day { get; set; }

        [ElementLocator(Id = "DateOfBirth_Month")]
        public IWebElement Month { get; set; }

        [ElementLocator(Id = "DateOfBirth_Year")]
        public IWebElement Year { get; set; }

    }
}
