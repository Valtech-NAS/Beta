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

        //[ElementLocator(Class = "date-input")]
        //public DateOfBirthTemplate DateOfBirth { get; set; }

        //todo:Add address template
        //public AddressTemplate Address { get; set; }

        [ElementLocator(Id = "EmailAddress")]
        public IWebElement EmailAddress { get; set; }


        [ElementLocator(Id = "Phonenumber")]
        public IWebElement Phonenumber { get; set; }

        [ElementLocator(Id = "Password")]
        public IWebElement Password { get; set; }

        [ElementLocator(Id = "HasAcceptedTermsAndConditions")]
        public IWebElement HasAcceptedTermsAndConditions { get; set; }

    }
}
