namespace SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships
{
    using System;

    public class ApprenticeshipSummary : VacancySummary
    {
        [Obsolete("Required for backward compatibility with previous document version that has this field.")]
        public string VacancyType {
            get { return null; }
            set { }
        }
    }
}
