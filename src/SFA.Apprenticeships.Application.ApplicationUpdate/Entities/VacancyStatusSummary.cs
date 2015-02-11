namespace SFA.Apprenticeships.Application.ApplicationUpdate.Entities
{
    using System;
    using Domain.Entities.Vacancies;

    public class VacancyStatusSummary
    {
        public int LegacyVacancyId { get; set; }

        public VacancyStatuses VacancyStatus { get; set; }

        public DateTime ClosingDate { get; set; }
    }
}
