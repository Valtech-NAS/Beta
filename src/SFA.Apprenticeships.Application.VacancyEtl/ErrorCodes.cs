namespace SFA.Apprenticeships.Application.VacancyEtl
{
    public static class ErrorCodes
    {
        //todo: use meaningful/consistent error code strings. need more specific error codes for these:
        public const string GatewayServiceFailed = "VacancyEtl.GatewayServiceFailed";
        public const string VacancyIndexerServiceError = "VacancyEtl.VacancyIndexerServiceError";
    }
}
