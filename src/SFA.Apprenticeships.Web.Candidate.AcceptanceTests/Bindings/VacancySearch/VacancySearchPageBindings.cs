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

        [Then(@"I clear the location")]
        public void ThenIClearTheLocation()
        {
            var location = _driver.FindElement(By.Id("Location"));
            location.Clear();
        }
 
    }
}