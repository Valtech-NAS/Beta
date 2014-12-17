namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Application.SummaryItems
{
    using OpenQA.Selenium;
    using SpecBind.Pages;
    using SpecBind.Selenium;
    using System.Linq;

    [ElementLocator(Class= "work-history-item")]
    public class WorkExperienceSummaryItem : WebElement
    {
        public WorkExperienceSummaryItem(ISearchContext parent) : base(parent)
        {
        }

        [ElementLocator(Class = "employer-name-span")]
        public IWebElement Employer { get; set; }

        [ElementLocator(Class = "job-title-span")]
        public IWebElement JobTitle { get; set; }

        [ElementLocator(Class = "main-duties-span")]
        public IWebElement MainDuties { get; set; }

        [ElementLocator(Class = "remove-work-experience-link")]
        public IWebElement RemoveLink    { get; set; }
    }
}