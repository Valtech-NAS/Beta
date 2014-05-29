namespace SFA.Apprenticeships.Services.VacancyEtl.Entities
{
    using System;
    using SFA.Apprenticeships.Domain.Interfaces.Enums;

    public class VacancySummaryPage
    {
        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public VacancyLocationType VacancyLocation { get; set; }

        public Guid UpdateReference { get; set; }
    }
}
