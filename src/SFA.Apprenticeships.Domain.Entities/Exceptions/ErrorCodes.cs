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
        public const string ApplicationViewIdNotFound = "Application004";
        public const string ApplicationCreationError = "Application005";
        public const string ApplicationDuplicatedError = "Application006";
        public const string ApplicationGatewayCreationError = "Application007";


        // Vacancy Exception Codes
        public const string VacancyNotFoundError = "Vacancy001";
        public const string VacancyExpired = "Vacancy002";

        // LDAP Exception Codes
        public const string LdapAccountExistError = "LDAP001";
        public const string LdapAccountNotFoundError = "LDAP002";
        public const string LdapEmptyNewPasswordError = "LDAP003";
        public const string LdapModifyPasswordError = "LDAP004";
        public const string LdapSetPasswordError = "LDAP005";
    }
}
