namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.SpecFlow.Steps.Vacancies
{
    using Common;
    using Pages;
    using TechTalk.SpecFlow;

    [Binding]
    public class VacancyDetailsSteps : CommonSteps
    {
        public VacancyDetailsSteps(VacancyDetailsPage vacancyDetails)
        {
            Page = vacancyDetails;
        }

        [Then(@"I expect to see a populated vacancy detail page")]
        public void ThenIExpectToSeeAPopulatedVacancyDetailPage()
        {
            Page.Verify();
        }
    }
}
