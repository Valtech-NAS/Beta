namespace SFA.Apprenticeships.Web.Candidate.IntegrationTests.SpecFlow.Steps.Vacancies
{
    using Common;
    using FluentAutomation;
    using Pages;
    using TechTalk.SpecFlow;

    [Binding]
    public class VacancyDetailsSteps : CommonSteps<VacancyDetailsPage>
    {
        private readonly VacancyDetailsPage _vacancyDetails;

        public VacancyDetailsSteps(FluentTest test, VacancyDetailsPage vacancyDetails) : base(test)
        {
            _vacancyDetails = vacancyDetails;
        }

        [Then(@"I expect to see a populated vacancy detail page")]
        public void ThenIExpectToSeeAPopulatedVacancyDetailPage()
        {
            _vacancyDetails.Verify();
        }
    }
}
