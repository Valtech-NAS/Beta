namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    public static class ErrorCodes
    {
        // Email Exception Codes
        public const string EmailError = "SendEmail.Failed";

        // Sms Exception Codes
        public const string SmsError = "SendSms.Failed";

        // Vacancy Exception Codes
        public const string VacancyNotFoundError = "VacancyNotFound.Error";

        // Application Exception Codes
        public const string ApplicationQueuingError = "ApplicationQueuing.Failed";
    }
}
