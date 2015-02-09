namespace SFA.Apprenticeships.Application.Interfaces.Users
{
    public static class ErrorCodes
    {
        public const string UserPasswordResetCodeExpiredError = "User.UserPasswordResetCodeExpiredError";
        public const string UserAccountLockedError = "User.UserAccountLockedError";
        public const string UserActivationCodeError = "User.UserActivationCodeError";
        public const string UserPasswordResetCodeIsInvalid = "User.UserPasswordResetCodeIsInvalid";
        public const string UserCreationError = "User.UserCreationError";
        public const string UserResetPasswordError = "User.UserResetPasswordError";
        public const string UserChangePasswordError = "User.UserChangePasswordError";
        public const string UnknownUserError = "User.UnknownUserError";
        public const string AccountUnlockCodeExpired = "User.AccountUnlockCodeExpired";
        public const string AccountUnlockCodeInvalid = "User.AccountUnlockCodeInvalid";
        public const string UserDirectoryAccountExistsError = "User.UserDirectoryAccountExistsError";
        public const string UserDirectoryAccountDoesNotExistError = "User.UserDirectoryAccountDoesNotExistError";
    }
}