namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    public static class ErrorCodes
    {
        //todo: use meaningful/consistent error code strings
        public const string VacancyNotFoundError = "VacancyNotFound.Error";
        public const string VacanciesSearchFailed = "VacanciesSearch.Failed";
        public const string GetVacancyDetailsFailed = "GetVacancyDetails.Failed";
        public const string LegacyVacancyStateError = "LegacyVacancyState.Error";
    }
}
