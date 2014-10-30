namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Bindings
{
    using System;
    using System.Reflection;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Generators;
    using global::SpecBind.Helpers;
    using OpenQA.Selenium;
    using SpecBind.BrowserSupport;
    using SpecBind.Selenium;
    using StructureMap;
    using System.Linq;
    using TechTalk.SpecFlow;

    [Binding]
    public class DataGeneratorBinding : Steps
    {
        private const string EmailTokenId = "EmailToken";
        private const string ActivationTokenId = "ActivationToken";
        private const string InvalidPasswordTokenName = "InvalidPasswordToken";
        private const string Password = "?Password01!"; //TODO: remove duplication?
        private readonly ITokenManager _tokenManager;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IWebDriver _driver;
        private string _email;

        public DataGeneratorBinding(ITokenManager tokenManager, IBrowser browser)
        {
            _tokenManager = tokenManager;

#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            _userReadRepository = ObjectFactory.GetInstance<IUserReadRepository>();
#pragma warning restore 0618
            _driver = Driver(browser);
        }

        [Given("I have created a new email address")]
        [When("I have created a new email address")]
        public void GivenICreateANewUserEmailAddress()
        {
            _email = EmailGenerator.GenerateEmailAddress();
            _tokenManager.SetToken(EmailTokenId, _email);
            _tokenManager.SetToken(InvalidPasswordTokenName, new string(Password.Reverse().ToArray()));
        }

        [When(@"I select the first vacancy in location ""(.*)"" that can apply by this website")]
        public void WhenISelectTheFirstVacancyThatCanApplyByThisWebsite(string location)
        {
            SearchForAVacancyIn(location);

            var isLocalVacancy = false;

            const int numResults = 50;
            var i = 0;

            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(180));

            while (!isLocalVacancy)
            {
                if (i == numResults)
                {
                    throw new Exception("Can't find any suitable vacancy.");
                }

                // Click on the i-th search result
                _driver.FindElements(
                    By.CssSelector(".search-results__item .vacancy-link"))
                    .Skip(i++).First().Click();

                try
                {
                    _driver.FindElement(By.Id("apply-button"));
                    isLocalVacancy = true;
                    
                }
                catch
                {
                    //Go Back
                    _driver.Navigate().Back();
                }
            }
        }

        private void SearchForAVacancyIn(string location)
        {
            Given("I navigated to the VacancySearchPage page");
            When("I enter data", GetVacancySearchData(location));
            And("I choose Search");
            Then("I am on the VacancySearchResultPage page");
            When("I enter data", Get50ResultsPerPage());
            Then("I am on the VacancySearchResultPage page");
        }

        [When(@"I select the first vacancy in location ""(.*)"" that I can apply via the employer site")]
        public void WhenISelectTheFirstVacancyInLocationThatICanApplyViaTheEmployerSite(string location)
        {
            SearchForAVacancyIn(location);

            var isExternalVacancy = false;

            const int numResults = 50;
            var i = 0;

            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(180));

            while (!isExternalVacancy)
            {
                if (i == numResults)
                {
                    throw new Exception("Can't find any suitable vacancy.");
                }

                // Click on the i-th search result
                _driver.FindElements(
                    By.CssSelector(".search-results__item .vacancy-link"))
                    .Skip(i++).First().Click();

                try
                {
                    _driver.FindElement(By.Id("external-employer-website"));
                    isExternalVacancy = true;
                }
                catch
                {
                    //Go Back
                    _driver.Navigate().Back();
                }
            }
        }

        [Then(@"Another browser window is opened")]
        public void ThenAnotherBrowserWindowIsOpened()
        {
            _driver.WindowHandles.Count.Should().Be(2);
        }

        [When(@"I navigate to the details of the vacancy (.*)")]
        public void WhenINavigateToTheDetailsOfTheVacancy(int vacancyid)
        {
            var vacancySearchUri = new Uri(_driver.Url);
            var vacancyDetailsUri = 
                string.Format("{0}://{1}:{2}/vacancysearch/details/{3}", 
                vacancySearchUri.Scheme, vacancySearchUri.Host, vacancySearchUri.Port, vacancyid);
            _driver.Navigate().GoToUrl(vacancyDetailsUri);
        }


        private Table GetVacancySearchData(string location)
        {
            string[] header = { "Field", "Value" };
            string[] row1 = { "Location", location };
            var t = new Table(header);
            t.AddRow(row1);
            return t;
        }

        private Table Get50ResultsPerPage()
        {
            string[] header = { "Field", "Value" };
            string[] row1 = { "ResultsPerPageDropDown", "50 per page" };
            var t = new Table(header);
            t.AddRow(row1);
            return t;
        }

        [Given(@"I have registered a new candidate")]
        public void GivenIHaveRegisteredANewCandidate()
        {
            Given("I navigated to the RegisterCandidatePage page");
            When("I have created a new email address");
            And("I enter data", GetRegistrationData());
            And("I choose HasAcceptedTermsAndConditions");
            And("I choose FindAddresses");
            And("I wait for 30 seconds to see AddressSelectLabel");
            Then("I am on the RegisterCandidatePage page");

            When("I am on AddressDropdown list item matching criteria", GetAddressMatchingCriteria());
            And("I choose WrappedElement");
            When("I am on the RegisterCandidatePage page");
            When("I choose CreateAccountButton");
            Then("I wait 500 second for the ActivationPage page");
            When("I get the token for my newly created account");
            And("I enter data", GetActivationCodeData());
            And("I choose ActivateButton");
            Then("I wait 120 second for the VacancySearchPage page");
            And("I am on the VacancySearchPage page");
        }

        private Table GetRegistrationData()
        {
            string[] header = { "Field", "Value" };
            string[] row1 = { "Firstname", "Firstname" };
            string[] row2 = { "Lastname", "Lastname" };
            string[] row3 = { "Phonenumber", "07970523193" };
            string[] row4 = { "EmailAddress", "{EmailToken}" };
            string[] row5 = { "PostcodeSearch", "N7 8LS  " };
            string[] row6 = { "Day", "01" };
            string[] row7 = { "Month", "01" };
            string[] row8 = { "Year", "2000" };
            string[] row9 = { "Password", "?Password01!" };
            string[] row10 = { "ConfirmPassword", "?Password01!" };

            var t = new Table(header);

            t.AddRow(row1);
            t.AddRow(row2);
            t.AddRow(row3);
            t.AddRow(row4);
            t.AddRow(row5);
            t.AddRow(row6);
            t.AddRow(row7);
            t.AddRow(row8);
            t.AddRow(row9);
            t.AddRow(row10);

            return t;
        }

        private Table GetAddressMatchingCriteria()
        {
            string[] header = { "Field", "Rule", "Value" };
            string[] row1 = { "Text", "Equals", "Flat A, 6 Furlong Road" };
            var t = new Table(header);
            t.AddRow(row1);
            return t;
        }

        private Table GetActivationCodeData()
        {
            string[] header = { "Field", "Value" };
            string[] row1 = { "ActivationCode", "{ActivationToken}" };
            var t = new Table(header);
            t.AddRow(row1);
            return t;
        }

        [When("I get the token for my newly created account")]
        public void WhenIGetTokenForMyNewlyCreatedAccount()
        {
            var email = _tokenManager.GetTokenByKey(EmailTokenId);
            var user = _userReadRepository.Get(email);

            if (user != null)
            {
                _tokenManager.SetToken(ActivationTokenId, user.ActivationCode);
            }
        }

        [Then("I set my token here")]
        public void ThenISetMyTokenHere()
        {
            var user = _userReadRepository.Get(_email);

            if (user != null)
            {
                _tokenManager.SetToken(ActivationTokenId, user.ActivationCode);
            }
        }

        private static IWebDriver Driver(IBrowser browser)
        {
            var field = typeof(SeleniumBrowser).GetField("driver", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            var value = field.GetValue(browser);
            return (value as System.Lazy<IWebDriver>).Value;
        }
    }
}
