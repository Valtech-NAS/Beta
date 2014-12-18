namespace SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships
{
    using System;
    using Locations;

    public class ApprenticeshipVacancyDetail : VacancyDetail
    {
        #region Vacancy

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        public ApprenticeshipLocationType VacancyLocationType { get; set; }

        #endregion

        #region Employer

        #endregion

        #region Provider

        #endregion

        #region Candidate

        #endregion 
    }
}
