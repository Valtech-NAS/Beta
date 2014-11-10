using OpenQA.Selenium;
using SpecBind.BrowserSupport;
using TechTalk.SpecFlow;

namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Bindings
{
    [Binding]
    public class HomePageBinding
    {
        private readonly IWebDriver _driver;

        public HomePageBinding(IBrowser browser)
        {
            _driver = BindingUtils.Driver(browser);
        }

        [Given(@"I am logged out")]
        public void GivenIAmLoggedOut()
        {
            try
            {
                var signoutLink = _driver.FindElement(By.Id("signout-link"));
                signoutLink.Click();
            }
            catch (NoSuchElementException)
            {
            }
        }
    }
}