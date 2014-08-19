namespace SFA.Apprenticeships.Service.Vacancy.Types
{
    using System;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies;

    public class VacancySummary
    {
        public int Id { get; set; }

        public double Score { get; set; }

        public string Title { get; set; }

        public string EmployerName { get; set; }

        public DateTime ClosingDate { get; set; }

        public string Description { get; set; }

        public GeoPoint Location { get; set; }

        public VacancyLocationType VacancyLocationType { get; set; }
    }
}
