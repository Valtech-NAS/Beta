namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Bindings.Application
{
    using OpenQA.Selenium;
    using TechTalk.SpecFlow;

    [Binding]
    public class ApplicationPageBinding
    {
        private readonly ISearchContext _searchContext;

        public ApplicationPageBinding(ISearchContext searchContext)
        {
            _searchContext = searchContext;
        }

        [When(@"I enter employer question data if present")]
        public void WhenIEnterEmployerQuestionData(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                var fieldName = tableRow["Field"];
                var fieldValue = tableRow["Value"];

                var field = _searchContext.FindElement(By.Id(fieldName));

                if (field != null)
                {
                    field.SendKeys(fieldValue);
                }
            }   
        }
    }
}
