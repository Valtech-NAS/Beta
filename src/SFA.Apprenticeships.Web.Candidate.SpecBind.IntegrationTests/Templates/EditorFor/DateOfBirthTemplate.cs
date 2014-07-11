namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Templates.EditorFor
{
    using global::SpecBind.Pages;
    using global::SpecBind.Selenium;
    using OpenQA.Selenium;

    public class DateOfBirthTemplate : WebElement
    {
        public DateOfBirthTemplate(ISearchContext searchContext) : base(searchContext)
        {
        }

        [ElementLocator(Type = "input", Index = 1)]
        public IWebElement Day { get; set; }

        [ElementLocator(Type = "input", Index = 2)]
        public IWebElement Month { get; set; }

        [ElementLocator(Type = "input", Index = 3)]
        public IWebElement Year { get; set; }

    }
}
