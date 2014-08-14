namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    public static class PasswordResetViewModelMessages
    {
        public static class PasswordResetCodeMessages
        {
            public const string LabelText = "Enter code";
            public const string HintText = "";
            public const string RequiredErrorText = "'Password reset code' must be supplied";
            public const string LengthErrorText = "'Password reset code' must be a 6 character code";
            public const string CodeErrorText = "'Password reset code' is invalid, try again";
        }
        public static class PasswordMessages
        {
            public const string LabelText = "New password";
            public const string HintText = "Must contain letters, numbers, symbols and be at least 8 characters.";
            public const string RequiredErrorText = "'New Password' must be supplied";
            public const string LengthErrorText = "'New Password' must be at least 8 characters long";
            public const string WhiteListRegularExpression = Whitelists.PasswordWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'New Password' " + Whitelists.PasswordWhitelist.ErrorText;
            public const string FailedPasswordResetErrorText = "'Password' reset failed, try again";
        }
    }
}