namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Home
{
    using SpecBind.Pages;
    using OpenQA.Selenium;
    using SpecBind.Selenium;

    [PageNavigation("/helpdesk")]
    [PageAlias("HelpdeskPage")]
    public class HelpdeskPage : BaseValidationPage
    {
        private IElementList<IWebElement, EnquiryDropdownItem> _enquiryDropdown;

        public HelpdeskPage(ISearchContext context) : base(context)
        {

        }

        [ElementLocator(Id = "Name")]
        public IWebElement Name { get; set; }

        [ElementLocator(Id = "Email")]
        public IWebElement Email { get; set; }

        [ElementLocator(Id = "contact-subject")]
        public IElementList<IWebElement, EnquiryDropdownItem> EnquiryDropdown
        {
            get { return _enquiryDropdown; }
            set { _enquiryDropdown = value; }
        }

        [ElementLocator(Id = "Enquiry")]
        public IWebElement Enquiry { get; set; }

        [ElementLocator(Id = "Details")]
        public IWebElement Details { get; set; }

        [ElementLocator(Id="send-contact-form-button")]
        public IWebElement SendButton { get; set; }
    }

    [ElementLocator(TagName = "option")]
    public class EnquiryDropdownItem : WebElement
    {
        public EnquiryDropdownItem(ISearchContext parent)
            : base(parent)
        {
        }
    }
}