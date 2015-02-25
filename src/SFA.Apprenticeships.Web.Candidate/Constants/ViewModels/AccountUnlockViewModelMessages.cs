namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    public static class AccountUnlockViewModelMessages
    {
        public static class AccountUnlockCodeMessages
        {
            public const string LabelText = "Enter code";
            public const string HintText = "";
            public const string RequiredErrorText = "Please enter your unlock code";
            public const string LengthErrorText = "Unlock code must be 6 characters";
            public const string EmailLabelText = "Email address";
            public const string WhiteListRegularExpression = "^[A-Za-z0-9]+$";
            public const string WhiteListErrorText = "Unlock code contains invalid characters";
        }
    }
}