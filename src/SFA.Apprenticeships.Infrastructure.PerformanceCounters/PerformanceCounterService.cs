namespace SFA.Apprenticeships.Infrastructure.PerformanceCounters
{
    using System.Diagnostics;

    public class PerformanceCounterService : IPerformanceCounterService
    {
        private const string WebRolePerformanceCounterCategory = "SFA.Apprenticeships.Web.Candidate";
        private const string VacancyEtlPerformanceCounterCategory = "SFA.Apprenticeships.WorkerRoles.VacancyEtl";
        private const string CandidateRegistrationCounter = "CandidateRegistration";
        private const string ApplicationSubmissionCounter = "ApplicationSubmission";
        private const string VacancySearchCounter = "VacancySearch";
        private const string VacancyIndexCounter = "VacancyEtlExecutions";

        public void IncrementCandidateRegistrationCounter()
        {
            IncrementWebRoleCounter(CandidateRegistrationCounter);
        }

        public void IncrementApplicationSubmissionCounter()
        {
            IncrementWebRoleCounter(ApplicationSubmissionCounter);
        }

        public void IncrementVacancySearchPerformanceCounter()
        {
            IncrementWebRoleCounter(VacancySearchCounter);
        }

        public void IncrementVacancyIndexPerformanceCounter()
        {
            IncrementVacancyEtlCounter(VacancyIndexCounter);
        }

        private static void IncrementWebRoleCounter(string performanceCounterName)
        {
            IncrementCounter(WebRolePerformanceCounterCategory, performanceCounterName);
        }

        private static void IncrementVacancyEtlCounter(string performanceCounterName)
        {
            IncrementCounter(VacancyEtlPerformanceCounterCategory, performanceCounterName);
        }

        private static void IncrementCounter(string categoryName, string performanceCounterName)
        {
            using (var counter = new PerformanceCounter(categoryName, performanceCounterName, false))
            {
                counter.Increment();
            }
        }
    }
}