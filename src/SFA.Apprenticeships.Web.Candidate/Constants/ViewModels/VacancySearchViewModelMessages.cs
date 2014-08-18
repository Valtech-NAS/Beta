namespace SFA.Apprenticeships.Web.Candidate.Constants.ViewModels
{
    using Common.Constants;

    public static class VacancySearchViewModelMessages
    {
        public static class KeywordMessages
        {
            public const string LabelText = "Keywords (optional)";
            public const string HintText = "For example, retail, customer service";
            public const string WhiteList = Whitelists.NameWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Keywords' " + Whitelists.NameWhitelist.ErrorText;
        }

        public static class LocationMessages
        {
            public const string LabelText = "Apprenticeship location";
            public const string HintText = "Enter postcode, town or city";
            public const string RequiredErrorText = "'Location' must be provided";
            public const string LengthErrorText = "'Location' must be 3 or more characters.";
            public const string WhiteList = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "'Apprenticeship location' " + Whitelists.NameWhitelist.ErrorText;
        }
    }
}