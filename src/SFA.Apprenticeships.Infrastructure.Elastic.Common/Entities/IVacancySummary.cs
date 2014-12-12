namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities
{
    using System;

    public interface IVacancySummary
    {
        int Id { get; }

        string Title { get; set; }

        DateTime ClosingDate { get; set; }

        string EmployerName { get; set; }

        string Description { get; set; }

        GeoPoint Location { get; set; }
    }
}
