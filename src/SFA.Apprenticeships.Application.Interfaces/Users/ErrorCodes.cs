namespace SFA.Apprenticeships.Application.Interfaces.Users
{
    public static class ErrorCodes
    {
        //todo: use meaningful/consistent error code strings
        public const string UserInIncorrectStateError = "UserInIncorrectState";
        public const string UserPasswordResetCodeExpiredError = "UserPasswordResetCodeExpired";
        public const string UserAccountLockedError = "UserAccountLocked";
        public const string UserActivationCodeError = "UserActivationCode";
        public const string UserPasswordResetCodeIsInvalid = "UserPasswordResetCodeIsInvalid";
        public const string UserCreationError = "UserCreation";
        public const string UserResetPasswordError = "UserResetPassword";
        public const string UserChangePasswordError = "UserChangePassword";
        public const string UnknownUserError = "UnknownUser";
        public const string AccountUnlockCodeExpired = "AccountUnlockCode.Expired";
        public const string AccountUnlockCodeInvalid = "AccountUnlockCode.Invalid";
        public const string UserDirectoryAccountExistsError = "UserDirectory.AccountExists";
        public const string UserDirectoryAccountDoesNotExistError = "UserDirectory.AccountDoesNotExist";
    }
}