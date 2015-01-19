namespace SFA.Apprenticeships.Application.VacancyEtl.Entities
{
    using System;
    using Domain.Entities.Vacancies;

    public class VacancyStatusSummary
    {
        public int LegacyVacancyId { get; set; }

        public VacancyStatuses VacancyStatus { get; set; }

        public DateTime ClosingDate { get; set; }

        public DateTime DateTime { get; set; }
    }
}
