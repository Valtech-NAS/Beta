using System.Collections.Generic;

namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Bindings
{
    using System;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Generators;
    using global::SpecBind.Helpers;
    using OpenQA.Selenium;
    using SpecBind.BrowserSupport;
    using StructureMap;
    using System.Linq;
    using TechTalk.SpecFlow;

    [Binding]
    public class DataGeneratorBinding : Steps
    {
        private const string EmailTokenId = "EmailToken";
        private const string ActivationTokenId = "ActivationToken";
        private const string InvalidPasswordTokenName = "InvalidPasswordToken";
        private const string VacancyIdToken = "VacancyId";
        private const string VacancyReferenceToken = "VacancyReference";
        private const string Password = "?Password01!"; //TODO: remove duplication?
        private readonly ITokenManager _tokenManager;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IWebDriver _driver;
        private readonly Dictionary<string, int> _positions = new Dictionary<string, int>
        {
            {"first", 1},
            {"second", 2}
        };
        private string _email;

        public DataGeneratorBinding(ITokenManager tokenManager, IBrowser browser)
        {
            _tokenManager = tokenManager;

#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            _userReadRepository = ObjectFactory.GetInstance<IUserReadRepository>();
#pragma warning restore 0618
            _driver = BindingUtils.Driver(browser);
        }

        [Given("I have created a new email address")]
        [When("I have created a new email address")]
        public void GivenICreateANewUserEmailAddress()
        {
            _email = EmailGenerator.GenerateEmailAddress();
            _tokenManager.SetToken(EmailTokenId, _email);
            _tokenManager.SetToken(InvalidPasswordTokenName, new string(Password.Reverse().ToArray()));
        }

        [Given(@"I select the ""(.*)"" apprenticeship vacancy in location ""(.*)""")]
        [When(@"I select the ""(.*)"" apprenticeship vacancy in location ""(.*)""")]
        public void WhenISelectTheNthApprenticeshipVacancy(string position, string location)
        {
            var expectedPosition = _positions[position];
            SearchForAnApprenticeshipVacancyIn(location);

            SelectVacancyInPosition(expectedPosition);
        }

        [Given(@"I select the ""(.*)"" apprenticeship vacancy in location ""(.*)"" that can apply by this website")]
        [When(@"I select the ""(.*)"" apprenticeship vacancy in location ""(.*)"" that can apply by this website")]
        public void WhenISelectTheNthApprenticeshipVacancyThatCanApplyByThisWebsite(string position, string location)
        {
            var expectedPosition = _positions[position];
            SearchForAnApprenticeshipVacancyIn(location);

            SelectVacancyApplicableViaThisWebsiteInPosition(expectedPosition);
        }

        [Given(@"I select the ""(.*)"" traineeship vacancy in location ""(.*)"" that can apply by this website")]
        [When(@"I select the ""(.*)"" traineeship vacancy in location ""(.*)"" that can apply by this website")]
        public void WhenISelectTheNthTraineeshipVacancyThatCanApplyByThisWebsite(string position, string location)
        {
            var expectedPosition = _positions[position];
            SearchForATraineeshipVacancyIn(location);

            SelectVacancyApplicableViaThisWebsiteInPosition(expectedPosition);
        }

        private void SelectVacancyInPosition(int expectedPosition)
        {
            const int numResults = 50;
            var i = 0;
            var validPositionCount = 0;

            while (validPositionCount != expectedPosition)
            {
                if (i == numResults)
                {
                    throw new Exception("Can't find any suitable apprenticeship vacancy.");
                }

                // Click on the i-th search result
                _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(180));
                var searchResult = _driver.FindElements(
                    By.CssSelector(".search-results__item .vacancy-link"))
                    .Skip(i++).First();
                var vacancyId = searchResult.GetAttribute("data-vacancy-id");
                _tokenManager.SetToken(VacancyIdToken, vacancyId);
                searchResult.Click();

                var vacancyReference = _driver.FindElement(By.Id("vacancy-reference-id")).Text;
                _tokenManager.SetToken(VacancyReferenceToken, vacancyReference);
                validPositionCount++;
                if (validPositionCount != expectedPosition)
                    _driver.Navigate().Back();
            }
        }

        private void SelectVacancyApplicableViaThisWebsiteInPosition(int expectedPosition)
        {
            const int numResults = 50;
            var i = 0;
            var validPositionCount = 0;

            while (validPositionCount != expectedPosition)
            {
                if (i == numResults)
                {
                    throw new Exception("Can't find any suitable apprenticeship vacancy.");
                }

                // Click on the i-th search result
                _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(180));
                var searchResult = _driver.FindElements(
                    By.CssSelector(".search-results__item .vacancy-link"))
                    .Skip(i++).First();
                var vacancyId = searchResult.GetAttribute("data-vacancy-id");
                _tokenManager.SetToken(VacancyIdToken, vacancyId);
                searchResult.Click();

                try
                {
                    _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
                    _driver.FindElement(By.Id("apply-button"));
                    var vacancyReference = _driver.FindElement(By.Id("vacancy-reference-id")).Text;
                    _tokenManager.SetToken(VacancyReferenceToken, vacancyReference);
                    validPositionCount++;
                    if (validPositionCount != expectedPosition)
                        _driver.Navigate().Back();
                }
                catch
                {
                    //Go Back
                    _driver.Navigate().Back();
                }
            }
        }

        [When(@"I select the ""(.*)"" apprenticeship vacancy in location ""(.*)"" that I can apply via the employer site")]
        public void WhenISelectTheNthVacancyInLocationThatICanApplyViaTheEmployerSite(string position, string location)
        {
            var expectedPosition = _positions[position];
            SearchForAnApprenticeshipVacancyIn(location);

            const int numResults = 50;
            var i = 0;
            var validPositionCount = 0;

            while (validPositionCount != expectedPosition)
            {
                if (i == numResults)
                {
                    throw new Exception("Can't find any suitable vacancy.");
                }

                // Click on the i-th search result
                _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(180));
                var searchResult = _driver.FindElements(
                    By.CssSelector(".search-results__item .vacancy-link"))
                    .Skip(i++).First();
                var vacancyId = searchResult.GetAttribute("data-vacancy-id");
                _tokenManager.SetToken(VacancyIdToken, vacancyId);
                searchResult.Click();

                try
                {
                    _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
                    _driver.FindElement(By.Id("external-employer-website"));
                    var vacancyReference = _driver.FindElement(By.Id("vacancy-reference-id")).Text;
                    _tokenManager.SetToken(VacancyReferenceToken, vacancyReference);
                    validPositionCount++;
                    if (validPositionCount != expectedPosition)
                        _driver.Navigate().Back();
                }
                catch
                {
                    //Go Back
                    _driver.Navigate().Back();
                }
            }
        }


        private void SearchForAnApprenticeshipVacancyIn(string location)
        {
            Given("I navigated to the ApprenticeshipSearchPage page");
            When("I enter data", GetVacancySearchData(location));
            And("I choose Search");
            Then("I am on the ApprenticeshipSearchResultPage page");
            When("I enter data", Get50ResultsPerPage());
            Then("I am on the ApprenticeshipSearchResultPage page");
        }

        private void SearchForATraineeshipVacancyIn(string location)
        {
            Given("I navigated to the TraineeshipSearchPage page");
            When("I enter data", GetVacancySearchData(location));
            And("I choose Search");
            Then("I am on the TraineeshipSearchResultPage page");
            When("I enter data", Get50ResultsPerPage());
            Then("I am on the TraineeshipSearchResultPage page");
        }

        [Then(@"Another browser window is opened")]
        public void ThenAnotherBrowserWindowIsOpened()
        {
            _driver.WindowHandles.Count.Should().Be(2);
        }

        [When(@"I navigate to the details of the apprenticeship vacancy (.*)")]
        public void WhenINavigateToTheDetailsOfTheApprenticeshipVacancy(int vacancyid)
        {
            var vacancySearchUri = new Uri(_driver.Url);
            var vacancyDetailsUri =
                string.Format("{0}://{1}:{2}/apprenticeship/{3}", 
                vacancySearchUri.Scheme, vacancySearchUri.Host, vacancySearchUri.Port, vacancyid);

            _driver.Navigate().GoToUrl(vacancyDetailsUri);
        }

        [When(@"I navigate to the details of the traineeship vacancy (.*)")]
        public void WhenINavigateToTheDetailsOfTheTraineeshipVacancy(int vacancyid)
        {
            var vacancySearchUri = new Uri(_driver.Url);
            var vacancyDetailsUri =
                string.Format("{0}://{1}:{2}/traineeship/{3}",
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
            Then("I wait 120 second for the ApprenticeshipSearchPage page");
            And("I am on the ApprenticeshipSearchPage page");
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
    }
}
