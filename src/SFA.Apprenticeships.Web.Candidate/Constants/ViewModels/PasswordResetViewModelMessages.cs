namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using Common.Constants;

    public static class PasswordResetViewModelMessages
    {
        public static class PasswordResetCodeMessages
        {
            public const string LabelText = "Enter code";
            public const string HintText = "";
            public const string RequiredErrorText = "Please enter password reset code";
            public const string LengthErrorText = "Password reset code must be 6 characters";
            public const string WhiteListRegularExpression = Whitelists.CodeWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Password reset code " + Whitelists.CodeWhitelist.ErrorText;
        }

        public static class PasswordMessages
        {
            public const string LabelText = "New password";
            public const string HintText = "Requires upper and lowercase letters, a number and at least 8 characters";
            public const string RequiredErrorText = "Please enter valid password";
            public const string LengthErrorText = "New password must be at least 8 characters";
            public const string WhiteListRegularExpression = Whitelists.PasswordWhitelist.RegularExpression;
            public const string WhiteListErrorText = "New password " + Whitelists.PasswordWhitelist.ErrorText;
            public const string PasswordsDoNotMatchErrorText = "Sorry, your passwords don’t match";
        }

        public static class ConfirmPasswordMessages
        {
            public const string LabelText = "Confirm password";
            public const string RequiredErrorText = "Please confirm password";
        }
    }
}
