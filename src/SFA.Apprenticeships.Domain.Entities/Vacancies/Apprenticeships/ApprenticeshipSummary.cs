namespace SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships
{
    using System;

    public class ApprenticeshipSummary : VacancySummary
    {
        public ApprenticeshipLocationType VacancyLocationType { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }
    }
}
