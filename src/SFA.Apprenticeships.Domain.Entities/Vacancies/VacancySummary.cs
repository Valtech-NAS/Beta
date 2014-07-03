namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    using System;
    using Locations;

    public class VacancySummary
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string EmployerName { get; set; }

        public DateTime ClosingDate { get; set; }

        public string Description { get; set; }

        public GeoPoint Location { get; set; }

        public VacancyLocationType VacancyLocationType { get; set; }
    }
}