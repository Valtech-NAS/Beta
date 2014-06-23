namespace SFA.Apprenticeships.Domain.Entities.Vacancy
{
    using System;
    using Location;
   
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