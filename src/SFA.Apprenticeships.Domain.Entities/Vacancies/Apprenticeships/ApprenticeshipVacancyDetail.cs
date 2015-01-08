namespace SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships
{
    using System;

    public class ApprenticeshipVacancyDetail : VacancyDetail
    {
        #region Vacancy

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        public ApprenticeshipLocationType VacancyLocationType { get; set; }

        //TODO: Remove once NAS gatway service updated to return correct vacancy address with multi-location vacancies
        public bool IsMultiLocation { get; set; }

        #endregion

        #region Employer

        #endregion

        #region Provider

        #endregion

        #region Candidate

        #endregion 
    }
}
