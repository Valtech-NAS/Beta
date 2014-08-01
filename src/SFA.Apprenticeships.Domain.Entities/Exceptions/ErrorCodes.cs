namespace SFA.Apprenticeships.Domain.Entities.Exceptions
{
    public static class ErrorCodes
    {
        // User Exception Codes
        public const string UnknownUserError = "User001";
        public const string UserInIncorrectStateError = "User002";
        public const string UserPasswordResetCodeExpiredError = "User003";
        public const string UserAccountLockedError = "User004";
        public const string UserActivationCodeError = "User005";
        public const string UserPasswordResetCodeIsInvalid = "User006";

        // Candidate Exception Codes
        public const string UnknownCandidateError = "Candidate001";

        // Application Exception Codes
        public const string ApplicationNotFoundError = "Application001";
        public const string ApplicationInIncorrectStateError = "Application002";
        public const string ApplicationQueuingError = "Application003";

        // Vacancy Exception Codes
        public const string VacancyNotFoundError = "Vacancy001";
    }
}
