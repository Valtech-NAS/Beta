namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using Vacancies;
    using Vacancies.Apprenticeships;

    public class ApprenticeshipApplicationDetail : ApplicationDetail
    {
        public ApprenticeshipApplicationDetail()
        {
            Vacancy = new ApprenticeshipSummary();
            Status = ApplicationStatuses.Unknown;
        }

        public ApplicationStatuses Status { get; set; }

        public VacancyStatuses VacancyStatus { get; set; }

        public string WithdrawnOrDeclinedReason { get; set; }

        public string UnsuccessfulReason { get; set; }

        public ApprenticeshipSummary Vacancy { get; set; }
    }
}
