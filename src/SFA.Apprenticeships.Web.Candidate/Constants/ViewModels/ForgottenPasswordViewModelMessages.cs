namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    public static class ForgottenPasswordViewModelMessages
    {
        public static class EmailAddressMessages
        {
            public const string LabelText = "Email address";
            public const string HintText = "";
            public const string RequiredErrorText = "'Email address' must be supplied";
            public const string TooLongErrorText = "'Email address' must not exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.EmailAddressWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Email address' " + Whitelists.EmailAddressWhitelist.ErrorText;
        }
    }
}