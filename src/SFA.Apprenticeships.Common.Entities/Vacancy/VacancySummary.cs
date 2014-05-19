using System;
using System.Security.Policy;
using SFA.Apprenticeships.Common.Interfaces.Enums;

namespace SFA.Apprenticeships.Common.Entities.Vacancy
{
    public class VacancySummary
    {
        public string Reference { get; set; }
        public string Framework { get; set; }
        public string Title { get; set; }
        public VacancyType TypeOfVacancy { get; set; }
        public DateTime Created { get; set; }
        public DateTime ClosingDate { get; set; }
        public string EmployerName { get; set; }
        public string ProviderName { get; set; }
        public int NumberOfPositions { get; set; }
        public string Description { get; set; }
        public VacancyAddress Address { get; set; }
        public VacancyLocationType TypeOfLocation { get; set; }
        public string VacancyUrl { get; set; }
    }
}
