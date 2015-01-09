namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using Vacancies;

    public class ApprenticeshipApplicationSummary : ApplicationSummary
    {
        public ApplicationStatuses Status { get; set; }

        public VacancyStatuses VacancyStatus { get; set; }

        public string UnsuccessfulReason { get; set; }
    }
}