namespace SFA.Apprenticeships.Application.Interfaces.Users
{
    public static class ErrorCodes
    {
        //todo: use meaningful strings here!
        public const string UserInIncorrectStateError = "User002";
        public const string UserPasswordResetCodeExpiredError = "User003";
        public const string UserAccountLockedError = "User004";
        public const string UserActivationCodeError = "User005";
        public const string UserPasswordResetCodeIsInvalid = "User006";
        public const string UserCreationError = "User007";
        public const string UserResetPasswordError = "User008";
        public const string UserChangePasswordError = "User009";
        public const string UnknownUserError = "UserRepository.UserNotFound";

        public const string AccountUnlockCodeExpired = "AccountUnlockCode.Expired";
        public const string AccountUnlockCodeInvalid = "AccountUnlockCode.Invalid";

        public const string UserDirectoryAccountExistsError = "UserDirectory.AccountExists";
        public const string UserDirectoryAccountDoesNotExistError = "UserDirectory.AccountDoesNotExist";
    }
}