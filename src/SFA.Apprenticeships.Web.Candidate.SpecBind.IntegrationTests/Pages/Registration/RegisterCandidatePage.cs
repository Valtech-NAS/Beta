namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages.Registration
{
    using global::SpecBind.Pages;
    using OpenQA.Selenium;
    using Templates.EditorFor;

    [PageNavigation("/register/index")]
    [PageAlias("RegisterCandidatePage")]
    public class RegisterCandidatePage
    {
        private readonly ISearchContext _context;

        public RegisterCandidatePage(ISearchContext context)
        {
            _context = context;
        }

        [ElementLocator(Id = "Firstname")]
        public IWebElement Firstname { get; set; }

        [ElementLocator(Id = "Lastname")]
        public IWebElement Lastname { get; set; }

        [ElementLocator(Id = "EmailAddress")]
        public IWebElement EmailAddress { get; set; }

        [ElementLocator(Id = "Phonenumber")]
        public IWebElement Phonenumber { get; set; }

        [ElementLocator(Id = "Password")]
        public IWebElement Password { get; set; }

        [ElementLocator(Id = "HasAcceptedTermsAndConditions")]
        public IWebElement HasAcceptedTermsAndConditions { get; set; }

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
        public IWebElement Latitude { get { return Address.Latitude; } }
        public IWebElement Longitude { get { return Address.Longitude; } }

        #region Buttons

        public IWebElement FindAddresses { get { return Address.FindAddresses; } }

        IElementList<IWebElement, AddressDropdownItem> AddressDropdown { get { return Address.AddressDropdown; } }

        #endregion

        #endregion
    }
}
