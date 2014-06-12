namespace SFA.Apprenticeships.Application.VacancyEtl.Entities
{
    using System;
    using Domain.Entities.Vacancy;

    public class VacancySummaryPage
    {
        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public VacancyLocationType VacancyLocation { get; set; }

        public DateTime ScheduledRefreshDateTime { get; set; }
    }
}
