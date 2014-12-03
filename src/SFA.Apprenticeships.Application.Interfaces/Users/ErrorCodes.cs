namespace SFA.Apprenticeships.Application.Interfaces.Users
{
    public static class ErrorCodes
    {
        public const string ActivationCodeResendingFailed = "ActivationCodeResending.Failed";
        public const string AccountUnlockCodeExpired = "AccountUnlockCode.Expired";
        public const string AccountUnlockCodeInvalid = "AccountUnlockCode.Invalid";
        public const string UnknownUserError = "UserRepository.UserNotFound";

        // User directory Exception Codes
        public const string UserDirectoryAccountExistsError = "UserDirectory.AccountExists";
        public const string UserDirectoryAccountDoesNotExistError = "UserDirectory.AccountDoesNotExist";
        public const string UserDirectoryEmptyNewPasswordError = "UserDirectory.EmptyNewPassword";
        public const string UserDirectoryChangePasswordError = "UserDirectory.ChangePassword.Error";
        public const string UserDirectorySetPasswordError = "UserDirectory.SetPassword.Error";
    }
}