namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships;

    public class ApprenticeshipApplicationDetail : ApplicationDetail
    {
        public ApprenticeshipApplicationDetail()
        {
            Vacancy = new ApprenticeshipSummary();
            Status = ApplicationStatuses.Unknown;
        }

        public ApplicationStatuses Status { get; set; }

        public string WithdrawnOrDeclinedReason { get; set; }

        public string UnsuccessfulReason { get; set; }

        public ApprenticeshipSummary Vacancy { get; set; }
    }
}