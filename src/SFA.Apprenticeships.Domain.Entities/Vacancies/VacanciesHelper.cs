namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    using System;

    public static class VacanciesHelper
    {
        public static bool IsExpired(this VacancyDetail vacancyDetail)
        {
            return IsBeforeToday(vacancyDetail.ClosingDate);
        }

        public static bool IsExpired(this VacancySummary vacancySummary)
        {
           return IsBeforeToday(vacancySummary.ClosingDate);
        }

        #region Helpers

        private static bool IsBeforeToday(DateTime date)
        {
            return date < DateTime.Today.ToUniversalTime();
        }

        #endregion
    }
}
