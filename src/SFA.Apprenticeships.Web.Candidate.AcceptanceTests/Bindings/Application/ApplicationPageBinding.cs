namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Bindings.Application
{
    using OpenQA.Selenium;
    using SpecBind.BrowserSupport;
    using TechTalk.SpecFlow;

    [Binding]
    public class ApplicationPageBinding
    {
        private readonly IWebDriver _driver;

        public ApplicationPageBinding(IBrowser browser)
        {
            _driver = BindingUtils.Driver(browser);
        }

        [When(@"I enter employer question data if present")]
        public void WhenIEnterEmployerQuestionData(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                IWebElement field = null;
                var fieldName = tableRow["Field"];
                var fieldValue = tableRow["Value"];

                try
                {
                    field = _driver.FindElement(By.Id(fieldName));
                }
                catch
                {
                }

                if (field != null)
                {
                    field.SendKeys(fieldValue);
                }
            }   
        }

        [When(@"I wait (.*) seconds")]
        public void WhenIWaitSeconds(int numSeconds)
        {
            System.Threading.Thread.Sleep(numSeconds * 1000);
        }
    }
}
