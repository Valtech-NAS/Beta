namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using Common.Constants;

    public static class ActivationCodeMessages
    {
        public static class ActivationCode
        {
            public const string LabelText = "Activation code";
            public const string HintText = "";
            public const string RequiredErrorText = "Please enter an activation code";
            public const string LengthErrorText = "Activation code must be 6 characters";
            public const string WhiteListRegularExpression = Whitelists.CodeWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Activation code " + Whitelists.CodeWhitelist.ErrorText;
        }

        public static class EmailAddressMessages
        {
            public const string LabelText = "Email address";
            public const string RequiredErrorText = "Please enter an email address";
        }
    }
}