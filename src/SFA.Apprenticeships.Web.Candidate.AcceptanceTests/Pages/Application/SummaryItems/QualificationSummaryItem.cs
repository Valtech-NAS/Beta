namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Application.SummaryItems
{
    using OpenQA.Selenium;
    using SpecBind.Pages;
    using SpecBind.Selenium;
    using System.Linq;

    [ElementLocator(TagName = "tr")]
    public class QualificationSummaryItem : WebElement
    {
        public QualificationSummaryItem(ISearchContext parent)
            : base(parent)
        {
        }

        [ElementLocator(Class = "qual-subject")]
        public IWebElement Subject { get; set; }

        [ElementLocator(Class = "qual-year")]
        public IWebElement Year { get; set; }

        [ElementLocator(Class = "qual-grade")]
        public IWebElement Grade { get; set; }

        [ElementLocator(Class = "qual-predicted")]
        public IWebElement Predicted { get; set; }

        [ElementLocator(Class = "edit-qualification-link")]
        public IWebElement EditQualificationLink { get; set; }

        [ElementLocator(Class = "remove-qualification-link")]
        public IWebElement RemoveQualificationLink { get; set; }
    }
}