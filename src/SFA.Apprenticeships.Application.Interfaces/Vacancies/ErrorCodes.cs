namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    public static class ErrorCodes
    {
        //todo: use meaningful/consistent error code strings
        public const string VacancyNotFoundError = "Vacancy.VacancyNotFoundError";
        public const string VacanciesSearchFailed = "Vacancy.VacanciesSearchFailed";
        public const string GetVacancyDetailsFailed = "Vacancy.GetVacancyDetailsFailed";
        public const string LegacyVacancyStateError = "Vacancy.LegacyVacancyStateError";
    }
}
