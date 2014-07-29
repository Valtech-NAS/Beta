namespace SFA.Apprenticeships.Domain.Entities.Applications
{
    using System;
    using Vacancies;

    public class ApplicationStatusSummary
    {
        public Guid? ApplicationId { get; set; }

        public int LegacyApplicationId { get; set; }

        public ApplicationStatuses ApplicationStatus { get; set; }

        public int LegacyVacancyId { get; set; }

        public VacancyStatuses VacancyStatus { get; set; }

        public DateTime ClosingDate { get; set; }

        public string WithdrawnOrDeclinedReason { get; set; } //todo: this relates to the candidate withdrawing or declining

        public string UnsuccessfulReason { get; set; } //todo: this relates to the vacancy manager rejecting a candidate's application
    }
}
