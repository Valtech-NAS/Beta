namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    public static class EnterPasswordResetCodeViewModelMessages
    {
        public static class PasswordResetCode
        {
            public const string LabelText = "Enter code";
            public const string HintText = "";
            public const string RequiredErrorText = "'Password reset code' must be supplied";
            public const string LengthErrorText = "'Password reset code' must be a 6-digit code";
            public const string WrongPasswordResetCodeErrorText = "'Password reset code' supplied is incorrect";
        }
    }
}