namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Pages.Registration
{
    using global::SpecBind.Pages;
    using global::SpecBind.Selenium;
    using OpenQA.Selenium;
    using Templates.EditorFor;

    [PageNavigation("/register")]
    [PageAlias("RegisterCandidatePage")]
    public class RegisterCandidatePage : BaseValidationPage
    {
        private IElementList<IWebElement, AddressDropdownItem> _addressDropdown;

        public RegisterCandidatePage(ISearchContext context) : base(context)
        {
        }

        [ElementLocator(Id = "Firstname")]
        public IWebElement Firstname { get; set; }

        [ElementLocator(Id = "Lastname")]
        public IWebElement Lastname { get; set; }

        [ElementLocator(Id = "EmailAddress")]
        public IWebElement EmailAddress { get; set; }

        [ElementLocator(Id = "PhoneNumber")]
        public IWebElement Phonenumber { get; set; }

        [ElementLocator(Id = "Password")]
        public IWebElement Password { get; set; }

        [ElementLocator(Id = "ConfirmPassword")]
        public IWebElement ConfirmPassword { get; set; }

        [ElementLocator(Id = "HasAcceptedTermsAndConditions")]
        public IWebElement HasAcceptedTermsAndConditions { get; set; }

        [ElementLocator(Id = "create-account-btn")]
        public IWebElement CreateAccountButton { get; set; }

        [ElementLocator(Id = "email-available-message")]
        public IWebElement EmailAddressAvailableMessage { get; set; }

        #region Date of birth

        [ElementLocator(Class = "date-input")]
        public DateOfBirthTemplate DateOfBirth { get; set; }

        public IWebElement Day { get { return DateOfBirth.Day; } }
        public IWebElement Month { get { return DateOfBirth.Month; } }
        public IWebElement Year { get { return DateOfBirth.Year; } }

        #endregion

        #region Address Template

        [ElementLocator(Id = "address-details")]
        public AddressTemplate Address { get; set; }

        public IWebElement AddressLine1 { get { return Address.AddressLine1; } }
        public IWebElement AddressLine2 { get { return Address.AddressLine2; } }
        public IWebElement AddressLine3 { get { return Address.AddressLine3; } }
        public IWebElement AddressLine4 { get { return Address.AddressLine4; } }
        public IWebElement Postcode { get { return Address.Postcode; } }

        public string Uprn { get { return Address.Uprn.GetAttribute("value"); } }

        public string Latitude { get { return Address.Latitude.GetAttribute("value"); } }

        public string Longitude { get { return Address.Longitude.GetAttribute("value"); } }

        #region Search Inputs

        [ElementLocator(Id = "postcode-search")]
        public IWebElement PostcodeSearch { get; set; }

        [ElementLocator(Id = "find-addresses")]
        public IWebElement FindAddresses { get; set; }

        [ElementLocator(Id = "address-select-label")]
        public IWebElement AddressSelectLabel { get; set; }

        [ElementLocator(Id = "address-select")]
        public IElementList<IWebElement, AddressDropdownItem> AddressDropdown
        {
            get { return _addressDropdown; }
            set { _addressDropdown = value; }
        }

        [ElementLocator(Id = "address-select")]
        public IWebElement Addresses { get; set; }

        #endregion

        #endregion
    }

    [ElementLocator(TagName = "option")]
    public class AddressDropdownItem : WebElement
    {
        public AddressDropdownItem(ISearchContext parent) : base(parent)
        {
        }

        public string AddressLine1 { get { return this.GetAttribute("data-address-line1"); } }
        public string AddressLine2 { get { return this.GetAttribute("data-address-line2"); } }
        public string AddressLine3 { get { return this.GetAttribute("data-address-line3"); } }
        public string AddressLine4 { get { return this.GetAttribute("data-address-line4"); } }
        public string Postcode { get { return this.GetAttribute("data-post-code"); } }
        public string Uprn { get { return this.GetAttribute("value"); } }
        public string Latitude { get { return this.GetAttribute("data-lat"); } }
        public string Longitude { get { return this.GetAttribute("data-lon"); } }
    }
}
