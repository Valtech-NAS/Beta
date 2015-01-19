namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using Vacancies.Apprenticeships;

    public class ApprenticeshipApplicationDetail : ApplicationDetail
    {
        public ApprenticeshipApplicationDetail()
        {
            Vacancy = new ApprenticeshipSummary();
        }

        public string WithdrawnOrDeclinedReason { get; set; }

        public string UnsuccessfulReason { get; set; }

        public ApprenticeshipSummary Vacancy { get; set; }
    }
}
