using OpenQA.Selenium;
using SpecBind.BrowserSupport;
using TechTalk.SpecFlow;

namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Bindings.VacancySearch
{
    [Binding]
    public class VacancySearchPageBindings
    {
        private readonly IWebDriver _driver;

        public VacancySearchPageBindings(IBrowser browser)
        {
            _driver = BindingUtils.Driver(browser);
        }

        [When(@"I clear the (.*) field")]
        [Then(@"I clear the (.*) field")]
        public void ThenIClearTheLocation(string fieldId)
        {
            var location = _driver.FindElement(By.Id(fieldId));
            location.Clear();
        }
    }
}