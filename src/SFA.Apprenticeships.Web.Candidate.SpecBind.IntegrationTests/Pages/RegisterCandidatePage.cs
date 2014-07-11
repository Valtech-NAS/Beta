namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Pages
{
    using global::SpecBind.Pages;
    using global::SpecBind.Selenium;
    using OpenQA.Selenium;

    [PageNavigation("/register/index")]
    [PageAlias("RegisterCandidatePage")]
    public class RegisterCandidatePage
    {
        private readonly ISearchContext _context;

        public RegisterCandidatePage(ISearchContext context)
        {
            _context = context;
        }


    }
}
