namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    using System;
    using Locations;

    public abstract class VacancySummary
    {
        public int Id { get; set; }

        public int VacancyReference { get; set; }
        
        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ClosingDate { get; set; }

        public string Description { get; set; }

        public string EmployerName { get; set; }

        public GeoPoint Location { get; set; }

        public string Sector { get; set; }

        public string Framework { get; set; }
    }
}
