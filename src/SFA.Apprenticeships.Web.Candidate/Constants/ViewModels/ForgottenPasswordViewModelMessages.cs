namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using Common.Constants;

    public static class ForgottenPasswordViewModelMessages
    {
        public static class EmailAddressMessages
        {
            public const string LabelText = "Email address";
            public const string HintText = "";
            public const string RequiredErrorText = "Please enter an email address";
            public const string TooLongErrorText = "Your email address mustn’t exceed 100 characters";
            public const string WhiteListRegularExpression = Whitelists.EmailAddressWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Your email address " + Whitelists.EmailAddressWhitelist.ErrorText;
        }
    }
}