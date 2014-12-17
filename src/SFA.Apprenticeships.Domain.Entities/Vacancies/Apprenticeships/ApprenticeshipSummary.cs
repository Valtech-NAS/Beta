namespace SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships
{
    using System;

    public class ApprenticeshipSummary : VacancySummary
    {
        public VacancyLocationType VacancyLocationType { get; set; }

        [Obsolete("Required for backward compatibility with previous document version that has this field.")]
        public int? VacancyType {
            get { return null; }
            set { }
        }
    }
}
