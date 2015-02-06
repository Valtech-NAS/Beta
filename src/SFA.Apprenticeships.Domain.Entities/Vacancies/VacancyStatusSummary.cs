namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    using System;

    public class VacancyStatusSummary
    {
        public int LegacyVacancyId { get; set; }

        public VacancyStatuses VacancyStatus { get; set; }

        public DateTime ClosingDate { get; set; }
    }
}
