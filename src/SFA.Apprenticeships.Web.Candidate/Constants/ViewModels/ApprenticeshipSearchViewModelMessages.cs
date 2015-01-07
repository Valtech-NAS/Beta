namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using Common.Constants;

    public static class ApprenticeshipSearchViewModelMessages
    {
        public static class KeywordMessages
        {
            public const string LabelText = "Keywords (optional)";
            public const string HintText = "";
            public const string WhiteList = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Keywords' " + Whitelists.NameWhitelist.ErrorText;
        }

        public static class LocationMessages
        {
            public const string LabelText = "Apprenticeship location";
            public const string HintText = "Enter postcode, town or city";
            public const string RequiredErrorText = "Please enter location";
            public const string LengthErrorText = "Location must be 2 or more characters";
            public const string WhiteList = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Location " + Whitelists.NameWhitelist.ErrorText;
        }
    }
}