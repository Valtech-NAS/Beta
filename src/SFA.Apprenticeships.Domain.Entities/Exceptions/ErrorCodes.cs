namespace SFA.Apprenticeships.Domain.Entities.Exceptions
{
    //todo: should not be in the domain project - error codes belong to Application.Services
    public static class ErrorCodes
    {
        // User Exception Codes
        public const string UnknownUserError = "User001";
        public const string UserInIncorrectStateError = "User002";
        public const string UserPasswordResetCodeExpiredError = "User003";
        public const string UserAccountLockedError = "User004";
        public const string UserActivationCodeError = "User005";
        public const string UserPasswordResetCodeIsInvalid = "User006";
        public const string UserCreationError = "User007";
        public const string UserResetPasswordError = "User008";
        public const string UserChangePasswordError = "User009";

        // Candidate Exception Codes
        public const string UnknownCandidateError = "Candidate001";

        // Application Exception Codes
        public const string ApplicationNotFoundError = "Application001";
        public const string ApplicationInIncorrectStateError = "Application002";
        public const string ApplicationQueuingError = "Application003";
        public const string ApplicationViewIdNotFound = "Application004";
        public const string ApplicationCreationError = "Application005";
        public const string ApplicationDuplicatedError = "Application006";
        public const string ApplicationGatewayCreationError = "Application007";

        // Vacancy Exception Codes
        public const string VacancyNotFoundError = "Vacancy001";
        public const string VacancyExpired = "Vacancy002";
        
        // Candidate Exception Codes
        public const string CandidateCreationError = "Candidate001";

        // Email Exception Codes
        public const string EmailSendGridError = "Email001";

        // Vacancy Exception Codes
        public const string VacancyIndexerServiceError = "VacancyIndexer001";
    }
}
