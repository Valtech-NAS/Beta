namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    public static class ActivationCodeMessages
    {
        public static class ActivationCode
        {
            public const string LabelText = "Activation code";
            public const string HintText = "";
            public const string RequiredErrorText = "Please enter an activation code";
            public const string LengthErrorText = "Activation code must be 6 characters";
            public const string WhiteListRegularExpression = "^[A-Za-z0-9]+$";
            public const string WhiteListErrorText = "Activation code contains invalid characters";
        }

        public static class EmailAddressMessages
        {
            public const string LabelText = "Email address";
            public const string RequiredErrorText = "Please enter an email address";
        }
    }
}