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
        }

        public static class PasswordMessages
        {
            public const string LabelText = "New password";
            public const string HintText = "Requires upper and lowercase letters, a number and at least 8 characters";
            public const string RequiredErrorText = "Please enter valid password reset code";
            public const string LengthErrorText = "New password must be at least 8 characters";
            public const string WhiteListRegularExpression = Whitelists.PasswordWhitelist.RegularExpression;
            public const string WhiteListErrorText = "New password " + Whitelists.PasswordWhitelist.ErrorText;
        }
    }
}
