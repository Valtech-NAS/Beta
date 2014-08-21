namespace SFA.Apprenticeships.Service.Vacancy.Types
{
    using System;

    public class VacancySummary
    {
        public int VacancyId { get; set; }

        public double Score { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string EmployerName { get; set; }

        public DateTime ClosingDate { get; set; }
    }
}
