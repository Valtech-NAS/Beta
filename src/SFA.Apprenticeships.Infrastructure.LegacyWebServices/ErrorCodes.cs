namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices
{
    public class ErrorCodes
    {
        public const string WebServiceFailed = "WebService.WebServiceFailed";

        // TODO: AG: retire.
        public static string GetCandidateInfoServiceFailed = "LegacyWebService.GetCandidateInfoServiceFailed";
        public static string GetVacancyDetailsServiceFailed = "LegacyWebService.GetVacancyDetailsServiceFailed";
        public static string GetVacancySummariesServiceFailed = "LegacyWebService.GetVacancySummariesServiceFailed";
        public static string GetApplicationsStatusServiceFailed = "LegacyWebService.GetApplicationsStatusServiceFailed";
    }
}